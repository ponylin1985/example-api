using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Infrastructure;
using Example.Api.Mappers;
using Example.Api.Models;
using Example.Api.Processes;
using Example.Api.Repositories;
using Example.Api.Services.DomainServices;

namespace Example.Api.Services;

/// <summary>
/// Service for managing patients.
/// </summary>
public class PatientService : BaseService, IPatientService
{
    /// <summary>
    /// Application logger factory.
    /// </summary>
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<PatientService> _logger;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// Repository for patient order history data access.
    /// </summary>
    private readonly IPatientOrderHistoryRepository _patientOrderHistoryRepository;

    /// <summary>
    /// Medication data repository.
    /// </summary>
    private readonly IMedicationRepository _medicationRepository;

    /// <summary>
    /// Order prescription policy for validations.
    /// </summary>
    private readonly IOrderPrescriptionPolicy _orderPrescriptionPolicy;

    /// <summary>
    /// Unit of work for managing transactions.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// The DateTimeOffset provider.
    /// </summary>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientService"/> class.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    /// <param name="patientRepository">The patient repository.</param>
    /// <param name="patientOrderHistoryRepository">The patient order history repository.</param>
    /// <param name="medicationRepository">The medication repository.</param>
    /// <param name="orderPrescriptionPolicy">The order prescription policy.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTimeOffsetProvider">The DateTimeOffset provider.</param>
    public PatientService(
        ILoggerFactory loggerFactory,
        IPatientRepository patientRepository,
        IPatientOrderHistoryRepository patientOrderHistoryRepository,
        IMedicationRepository medicationRepository,
        IOrderPrescriptionPolicy orderPrescriptionPolicy,
        IUnitOfWork unitOfWork,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<PatientService>();
        _patientRepository = patientRepository;
        _patientOrderHistoryRepository = patientOrderHistoryRepository;
        _medicationRepository = medicationRepository;
        _orderPrescriptionPolicy = orderPrescriptionPolicy;
        _unitOfWork = unitOfWork;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request)
    {
        _logger.LogInformation(
            "Querying patients from {StartTime} to {EndTime}, PageNumber: {PageNumber}, PageSize: {PageSize}",
            request.StartTime,
            request.EndTime,
            request.PageNumber,
            request.PageSize);

        var queryResult = await _patientRepository.GetPatientsAsync(
                request.PageNumber,
                request.PageSize,
                request.Name,
                request.IsPrefix,
                request.Email,
                request.PhoneNumber,
                request.Status,
                request.StartTime,
                request.EndTime);

        if (!HasPatientsData())
        {
            _logger.LogInformation(
                "No patients found for the given date range: {StartTime} to {EndTime}",
                request.StartTime,
                request.EndTime);
            return NoDataFoundPagedResult<PatientDto>();
        }

        var dtos = queryResult.Data!.ToDtos();
        return SuccessPagedResult(
            dtos,
            request.PageNumber,
            request.PageSize,
            queryResult.TotalCount);

        bool HasPatientsData() =>
            !queryResult.Data.IsNullOrEmpty() && queryResult.TotalCount > 0;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> GetPatientAsync(long id)
    {
        var patient = await _patientRepository.GetPatientAsync(id);

        if (patient is null or { Id: <= 0 })
        {
            _logger.LogWarning("Patient with ID {Id} was not found.", id);
            throw new BusinessException(ApiCode.NoDataFound, "Patient not found.");
        }

        return SuccessResult(patient!.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> AddPatientAsync(CreatePatientRequest request)
    {
        var process = new AddPatientProcess(
            _loggerFactory.CreateLogger<AddPatientProcess>(),
            request,
            _patientRepository,
            _patientOrderHistoryRepository,
            _orderPrescriptionPolicy,
            _dateTimeOffsetProvider);

        await _unitOfWork.ExecuteStrategyAsync(async () =>
        {
            try
            {
                using var _ = await _unitOfWork.BeginTransactionAsync();
                await process
                    .Prepare()
                    .EnsureEmailUniqueAsync()
                    .ThenAsync(p => p.EnsurePhoneNumberUniqueAsync())
                    .ThenAsync(p => p.EnsurePrescriptionValidAsync())
                    .ThenAsync(p => p.ExecuteAsync(_unitOfWork))
                    .Then(p => p.ShouldSuccessfully());
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new patient. Request: {@Request}", request);
                await _unitOfWork.RollbackTransactionAsync();
                throw new BusinessException(ApiCode.OperationFailed, "Failed to add new patient.");
            }
        });

        return SuccessResult(process.CreatedPatient!.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> UpdatePatientAsync(UpdatePatientRequest request)
    {
        Patient? updatedPatient = default;
        var patient = MapToEntity(request);

        await WhenUpdatingPatientAsync();
        ShouldUpdatedSuccessfully();
        return SuccessResult(updatedPatient!.ToDto(includeOrders: false));

        async Task WhenUpdatingPatientAsync()
        {
            updatedPatient = await _patientRepository.UpdateAsync(patient);
            await _unitOfWork.SaveChangesAsync();
        }

        void ShouldUpdatedSuccessfully()
        {
            if (updatedPatient is not { Id: > 0 })
            {
                _logger.LogError("Failed to update patient: {PatientId}", patient.Id);
                throw new BusinessException(ApiCode.OperationFailed, "Failed to update patient.");
            }
        }
    }

    /// <summary>
    /// Maps UpdatePatientRequest to Patient entity.
    /// </summary>
    /// <param name="request">The update patient request.</param>
    /// <returns>Patient entity.</returns>
    private static Patient MapToEntity(UpdatePatientRequest request)
    {
        var patient = new Patient
        {
            Id = request.Id,
            Name = request.Name!.Trim(),
            Age = request.Age!.Value,
            Gender = request.Gender!.Value,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
            PhoneNumber = request.PhoneNumber!.Trim(),
            DateOfBirth = request.DateOfBirth!.Value,
            Address = request.Address,
            Remarks = request.Remarks,
            UpdatedBy = request.UserId!.Trim(),
        };

        return patient;
    }
}
