using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Mappers;
using Example.Api.Models;
using Example.Api.Repositories;
using Example.Api.Services.DomainServices;

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
    private readonly IPatientRepository _patientRepository;

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
    /// <param name="medicationRepository">The medication repository.</param>
    /// <param name="orderPrescriptionPolicy">The order prescription policy.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTimeOffsetProvider">The DateTimeOffset provider.</param>
    public PatientService(
        ILogger<PatientService> logger,
        IPatientRepository patientRepository,
        IMedicationRepository medicationRepository,
        IOrderPrescriptionPolicy orderPrescriptionPolicy,
        IUnitOfWork unitOfWork,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _patientRepository = patientRepository;
        _medicationRepository = medicationRepository;
        _orderPrescriptionPolicy = orderPrescriptionPolicy;
        _unitOfWork = unitOfWork;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request)
    {
        (IEnumerable<Patient> Data, long TotalCount) queryResult = default;

        EnsureRequestValid();
        await WhenQueryingPatientsAsync();
        ShouldFoundPatients();

        var dtos = queryResult.Data!.ToDtos();
        return SuccessPagedResult(
            dtos,
            request.PageNumber,
            request.PageSize,
            queryResult.TotalCount);

        void EnsureRequestValid()
        {
            if (request is null)
            {
                _logger.LogError("GetPatientsRequest is null.");
                throw new BusinessException(ApiCode.InvalidRequest, "Request cannot be null.");
            }
        }

        async Task WhenQueryingPatientsAsync()
        {
            _logger.LogInformation(
                "Querying patients from {StartTime} to {EndTime}, PageNumber: {PageNumber}, PageSize: {PageSize}",
                request.StartTime,
                request.EndTime,
                request.PageNumber,
                request.PageSize);

            queryResult = await _patientRepository.GetPatientsAsync(
                request.StartTime,
                request.EndTime,
                request.PageNumber,
                request.PageSize);
        }

        void ShouldFoundPatients()
        {
            if (queryResult.Data is null || !queryResult.Data.Any() || queryResult.TotalCount == 0)
            {
                _logger.LogInformation(
                    "No patients found for the given date range: {StartTime} to {EndTime}",
                    request.StartTime,
                    request.EndTime);
                throw new BusinessException(ApiCode.NoDataFound, "No patients found.");
            }
        }
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> GetPatientAsync(long id)
    {
        long patientId = default;
        Patient? patient = default;

        GivenPatientId();
        await WhenQueryingPatientAsync();
        ShouldFoundPatient();
        return SuccessResult(patient!.ToDto());

        void GivenPatientId() => 
            patientId = id;

        async Task WhenQueryingPatientAsync() =>
            patient = await _patientRepository.GetPatientAsync(patientId);

        void ShouldFoundPatient()
        {
            if (patient is null)
            {
                _logger.LogWarning("Patient with ID {Id} was not found.", id);
                throw new BusinessException(ApiCode.NoDataFound, "Patient not found.");
            }
        }
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> AddPatientAsync(CreatePatientRequest request)
    {
        Patient? createdPatient = default;
        var patient = MapToEntity(request);

        await EnsureEmailUniqueAsync();
        await EnsurePhoneNumberUniqueAsync();
        await EnsurePrescriptionValidAsync();
        await WhenAddPatientAsync();
        ShouldCreatedSuccessfully();
        return SuccessResult(createdPatient!.ToDto());

        async Task EnsureEmailUniqueAsync()
        {
            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                return;
            }

            if (await _patientRepository.IsExistPatentByEmailAsync(patient.Email))
            {
                _logger.LogWarning("Email {Email} is already in use.", patient.Email);
                throw new BusinessException(ApiCode.OperationFailed, "Email is already in use.");
            }
        }

        async Task EnsurePhoneNumberUniqueAsync()
        {
            if (await _patientRepository.IsExistPatientByPhoneAsync(patient.PhoneNumber))
            {
                _logger.LogWarning("Phone number {PhoneNumber} is already in use.", patient.PhoneNumber);
                throw new BusinessException(ApiCode.OperationFailed, "Phone number is already in use.");
            }
        }

        async Task EnsurePrescriptionValidAsync()
        {
            await _orderPrescriptionPolicy.EnsureMedicationIdsValidAsync(patient.Orders.FirstOrDefault()!);
        }

        async Task WhenAddPatientAsync()
        {
            createdPatient = await _patientRepository.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();
        }

        void ShouldCreatedSuccessfully()
        {
            if (createdPatient is not { Id: > 0 })
            {   
                _logger.LogError("Failed to create patient: {Patient}", patient);
                throw new BusinessException(ApiCode.OperationFailed, "Failed to create patient.");
            }
        }
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
    /// Maps CreatePatientRequest to Patient entity.
    /// </summary>
    /// <param name="request">The create patient request.</param>
    /// <returns>Patient entity.</returns>
    private Patient MapToEntity(CreatePatientRequest request)
    {
        var userId = request.UserId!.Trim();

        var patient = new Patient
        {
            Name = request.Name!.Trim(),
            Age = request.Age!.Value,
            Gender = request.Gender!.Value,
            Email = request.Email?.Trim() ?? string.Empty,
            PhoneNumber = request.PhoneNumber!.Trim(),
            DateOfBirth = request.DateOfBirth!.Value,
            Address = request.Address,
            FirstVisitDate = _dateTimeOffsetProvider.UtcNow,
            Status = PatientStatus.Active,
            Remarks = request.Remarks,
            CreatedBy = userId,
            UpdatedBy = userId,
            Orders =
            [
                new()
                {
                    Instructions = request.Order!.Instructions!.Trim(),
                    NextVisitDate = request.Order.NextVisitDate?.UtcDateTime,
                    StartDate = request.Order.StartDate?.UtcDateTime,
                    EndDate = request.Order.EndDate?.UtcDateTime,
                    Type = request.Order.Type!.Value,
                    Status = OrderStatus.Created,
                    DispensedDate = request.Order.DispensedDate?.UtcDateTime,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    Prescriptions = request.Order.Prescriptions!.Select(p => new Prescription
                    {
                        MedicationId = p.MedicationId!.Value,
                        Dose = p.Dose!.Trim(),
                        Frequency = p.Frequency!.Trim(),
                        DurationInDays = p.DurationInDays!.Value,
                        Route = p.Route!.Value,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                    }).ToList(),
                },
            ],
        };

        return patient;
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
            Email = request.Email?.Trim() ?? string.Empty,
            PhoneNumber = request.PhoneNumber!.Trim(),
            DateOfBirth = request.DateOfBirth!.Value,
            Address = request.Address,
            Remarks = request.Remarks,
            UpdatedBy = request.UserId!.Trim(),
        };

        return patient;
    }
}
