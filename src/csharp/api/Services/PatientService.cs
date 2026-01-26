using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Infrastructure;
using Example.Api.Mappers;
using Example.Api.Models;
using Example.Api.Repositories;

namespace Example.Api.Services;

/// <summary>
/// Service for managing patients.
/// </summary>
public class PatientService : BaseService, IPatientService
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<PatientService> _logger;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _repository;

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
    /// <param name="repository">The patient repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTimeOffsetProvider">The DateTimeOffset provider.</param>
    public PatientService(
        ILogger<PatientService> logger,
        IPatientRepository repository,
        IUnitOfWork unitOfWork,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request)
    {
        var queryResult = await _repository.GetPatientsAsync(
            request.StartTime,
            request.EndTime,
            request.PageNumber,
            request.PageSize);

        if (IsNoDataFound())
        {
            _logger.LogInformation(
                "No patients found for the given date range: {StartTime} to {EndTime}",
                request.StartTime,
                request.EndTime);
            return NoDataFoundPagedResult<PatientDto>();
        }

        var dtos = queryResult.Data.ToDtos();
        return SuccessPagedResult(
            dtos,
            request.PageNumber,
            request.PageSize,
            queryResult.TotalCount);

        bool IsNoDataFound() =>
            queryResult.Data.IsNullOrEmpty() || queryResult.TotalCount == 0;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> GetPatientAsync(long id)
    {
        var patient = await _repository.GetPatientAsync(id);

        if (patient is null)
        {
            _logger.LogInformation("Patient with ID {Id} not found.", id);
            return NoDataFoundResult<PatientDto>();
        }

        return SuccessResult(patient.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> CreatePatientAsync(CreatePatientRequest request)
    {
        var patient = new Patient
        {
            Name = request.Name!.Trim(),
            Age = request.Age!.Value,
            Gender = request.Gender!.Value,
            Email = request.Email?.Trim() ?? string.Empty,
            PhoneNumber = request.PhoneNumber!.Trim(),
            DateOfBirth = request.DateOfBirth!.Value,
            FirstVisitDate = _dateTimeOffsetProvider.UtcNow,
            Status = PatientStatus.Active,
            Orders =
            [
                new()
                {
                    Instructions = request.Order!.Instructions!.Trim(),
                    NextVisitDate = request.Order.NextVisitDate,
                    StartDate = request.Order.StartDate,
                    EndDate = request.Order.EndDate,
                    Type = request.Order.Type!.Value,
                    Status = OrderStatus.Created,
                    DispensedDate = request.Order.DispensedDate,
                    Prescriptions = request.Order.Prescriptions!.Select(p => new Prescription
                    {
                        MedicationId = p.MedicationId!.Value,
                        Dose = p.Dose!.Trim(),
                        Frequency = p.Frequency!.Trim(),
                        DurationInDays = p.DurationInDays!.Value,
                        Route = p.Route!.Value,
                    }).ToList(),
                },
            ],
        };

        var createdPatient = await _repository.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        if (!IsCreatedSuccessfully())
        {
            _logger.LogError("Failed to create patient: {Patient}", patient);
            return FailureResult<PatientDto>(ApiCode.OperationFailed, "Failed to create patient.");
        }

        return SuccessResult(createdPatient.ToDto());

        bool IsCreatedSuccessfully() =>
            createdPatient.Id != default;
    }
}
