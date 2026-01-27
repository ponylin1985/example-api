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
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// Medication data repository.
    /// </summary>
    private readonly IMedicationRepository _medicationRepository;

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
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="dateTimeOffsetProvider">The DateTimeOffset provider.</param>
    public PatientService(
        ILogger<PatientService> logger,
        IPatientRepository patientRepository,
        IMedicationRepository medicationRepository,
        IUnitOfWork unitOfWork,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _patientRepository = patientRepository;
        _medicationRepository = medicationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request)
    {
        var queryResult = await _patientRepository.GetPatientsAsync(
            request.StartTime,
            request.EndTime,
            request.PageNumber,
            request.PageSize);

        ShouldFoundPatients();

        var dtos = queryResult.Data.ToDtos();
        return SuccessPagedResult(
            dtos,
            request.PageNumber,
            request.PageSize,
            queryResult.TotalCount);

        void ShouldFoundPatients()
        {
            if (queryResult.Data.IsNullOrEmpty() || queryResult.TotalCount == 0)
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
        var patient = await _patientRepository.GetPatientAsync(id);
        ShouldFoundPatient();
        return SuccessResult(patient!.ToDto());

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
        // BDD Style: Given (Guard Clauses)
        var patient = MapToEntity(request);
        await EnsureEmailUniqueAsync();
        await EnsurePhoneNumberUniqueAsync();
        await EnsurePrescriptionValidAsync();

        // BDD Style: When (Action)
        var createdPatient = await _patientRepository.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        // BDD Style: Then (Assertions)
        ShouldCreatedSuccessfully();
        return SuccessResult(createdPatient.ToDto());

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
            var medicationIds = patient.Orders
                .SelectMany(o => o.Prescriptions)
                .Select(p => p.MedicationId)
                .Distinct()
                .ToList();

            if (medicationIds.Count == 0)
            {
                throw new BusinessException(
                    ApiCode.OperationFailed, "At least one prescription with valid medication ID is required.");
            }

            var existingCount = await _medicationRepository.GetExistingMedicationCountAsync(medicationIds);

            if (existingCount != medicationIds.Count)
            {
                _logger.LogWarning(
                    "One or more medication IDs in prescriptions are invalid. Provided IDs: {MedicationIds}",
                    string.Join(", ", medicationIds));
                throw new BusinessException(
                    ApiCode.OperationFailed, "One or more prescriptions have invalid medication IDs.");
            }
        }

        void ShouldCreatedSuccessfully()
        {
            if (createdPatient.Id <= 0)
            {
                _logger.LogError("Failed to create patient: {Patient}", patient);
                throw new BusinessException(ApiCode.OperationFailed, "Failed to create patient.");
            }
        }
    }

    /// <summary>
    /// Add new patient.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ApiResult<PatientDto>> AddPatientAsync_ProcedureStyleBdd(CreatePatientRequest request)
    {
        var patient = MapToEntity(request);

        if (!await IsEmailDuplicatedAsync())
        {
            _logger.LogWarning("Email {Email} is already in use.", patient.Email);
            return FailureResult<PatientDto>(ApiCode.OperationFailed, "Email is already in use.");
        }

        if (!await IsPhoneNumberDuplicatedAsync())
        {
            _logger.LogWarning("Phone number {PhoneNumber} is already in use.", patient.PhoneNumber);
            return FailureResult<PatientDto>(ApiCode.OperationFailed, "Phone number is already in use.");
        }

        if (!await IsPrescriptionValidAsync())
        {
            _logger.LogWarning("One or more prescriptions have invalid medication IDs.");
            return FailureResult<PatientDto>(
                ApiCode.OperationFailed, "One or more prescriptions have invalid medication IDs.");
        }

        var createdPatient = await _patientRepository.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        if (!IsCreatedSuccessfully())
        {
            _logger.LogError("Failed to create patient: {Patient}", patient);
            return FailureResult<PatientDto>(ApiCode.OperationFailed, "Failed to create patient.");
        }

        return SuccessResult(createdPatient.ToDto());

        async Task<bool> IsEmailDuplicatedAsync()
        {
            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                return true;
            }

            var exists = await _patientRepository.IsExistPatentByEmailAsync(patient.Email);
            return !exists;
        }

        async Task <bool> IsPhoneNumberDuplicatedAsync()
        {
            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                return true;
            }

            var exists = await _patientRepository.IsExistPatientByPhoneAsync(patient.PhoneNumber);
            return !exists;
        }

        async Task<bool> IsPrescriptionValidAsync()
        {
            foreach (var order in patient.Orders)
            {
                foreach (var prescription in order.Prescriptions)
                {
                    if (!await _medicationRepository.IsExistMedicationAsync(prescription.MedicationId))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        bool IsCreatedSuccessfully() =>
            createdPatient.Id != default;
    }

    /// <summary>
    /// Maps CreatePatientRequest to Patient entity.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
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
}
