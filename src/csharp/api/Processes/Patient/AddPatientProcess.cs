using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos.Requests;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Example.Api.Repositories;
using Example.Api.Services.DomainServices;

namespace Example.Api.Processes;

/// <summary>
/// Encapsulates the process for adding a new patient, including validation and persistence steps.
/// </summary>

/// <summary>
/// Process for adding a patient.
/// </summary>
public sealed class AddPatientProcess
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<AddPatientProcess> _logger;

    /// <summary>
    /// The request DTO containing patient creation data.
    /// </summary>
    private readonly CreatePatientRequest _request;

    /// <summary>
    /// Repository for patient data access.
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// Policy for validating order prescriptions.
    /// </summary>
    private readonly IOrderPrescriptionPolicy _orderPrescriptionPolicy;

    /// <summary>
    /// Provider for current date and time.
    /// </summary>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// The patient entity prepared for creation.
    /// </summary>
    public Patient? Patient { get; private set; }

    /// <summary>
    /// The patient entity after successful creation.
    /// </summary>
    public Patient? CreatedPatient { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddPatientProcess"/> class.
    /// </summary>
    /// <param name="logger">The application logger.</param>
    /// <param name="request">The request DTO containing patient creation data.</param>
    /// <param name="patientRepository">Repository for patient data access.</param>
    /// <param name="orderPrescriptionPolicy">Policy for validating order prescriptions.</param>
    /// <param name="dateTimeOffsetProvider">Provider for current date and time.</param>
    public AddPatientProcess(
        ILogger<AddPatientProcess> logger,
        CreatePatientRequest request,
        IPatientRepository patientRepository,
        IOrderPrescriptionPolicy orderPrescriptionPolicy,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _request = request;
        _patientRepository = patientRepository;
        _orderPrescriptionPolicy = orderPrescriptionPolicy;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <summary>
    /// Prepares the patient entity from the request data.
    /// </summary>
    /// <returns>The current <see cref="AddPatientProcess"/> instance.</returns>
    public AddPatientProcess Prepare()
    {
        Patient = MapToEntity(_request);
        return this;
    }

    /// <summary>
    /// Ensures the patient's email is unique in the system.
    /// </summary>
    /// <returns>The current <see cref="AddPatientProcess"/> instance.</returns>
    /// <exception cref="BusinessException">Thrown if the email is already in use.</exception>
    public async Task<AddPatientProcess> EnsureEmailUniqueAsync()
    {
        if (!string.IsNullOrWhiteSpace(Patient!.Email) &&
            await _patientRepository.IsExistPatientByEmailAsync(Patient.Email))
        {
            _logger.LogWarning("Email {Email} is already in use.", Patient.Email);
            throw new BusinessException(ApiCode.OperationFailed, "Email is already in use.");
        }
        return this;
    }

    /// <summary>
    /// Ensures the patient's phone number is unique in the system.
    /// </summary>
    /// <returns>The current <see cref="AddPatientProcess"/> instance.</returns>
    /// <exception cref="BusinessException">Thrown if the phone number is already in use.</exception>
    public async Task<AddPatientProcess> EnsurePhoneNumberUniqueAsync()
    {
        if (await _patientRepository.IsExistPatientByPhoneAsync(Patient!.PhoneNumber))
        {
            _logger.LogWarning("Phone number {PhoneNumber} is already in use.", Patient.PhoneNumber);
            throw new BusinessException(ApiCode.OperationFailed, "Phone number is already in use.");
        }
        return this;
    }

    /// <summary>
    /// Ensures the prescriptions in the patient's order are valid.
    /// </summary>
    /// <returns>The current <see cref="AddPatientProcess"/> instance.</returns>
    public async Task<AddPatientProcess> EnsurePrescriptionValidAsync()
    {
        await _orderPrescriptionPolicy.EnsureMedicationIdsValidAsync(Patient!.Orders.FirstOrDefault()!);
        return this;
    }

    /// <summary>
    /// Executes the process to add the patient to the repository.
    /// </summary>
    /// <returns>The current <see cref="AddPatientProcess"/> instance.</returns>
    public async Task<AddPatientProcess> ExecuteAsync()
    {
        CreatedPatient = await _patientRepository.AddAsync(Patient!);
        return this;
    }

    /// <summary>
    /// Commits the changes to the database using the provided unit of work.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for transaction management.</param>
    /// <returns>The current <see cref="AddPatientProcess"/> instance.</returns>
    public async Task<AddPatientProcess> CommitAsync(IUnitOfWork unitOfWork)
    {
        await unitOfWork.SaveChangesAsync();
        return this;
    }

    /// <summary>
    /// Ensures the patient was created successfully.
    /// </summary>
    /// <exception cref="BusinessException">Thrown if the patient creation failed.</exception>
    public void ShouldSuccessfully()
    {
        if (CreatedPatient is null or { Id: <= 0 })
        {
            _logger.LogError("Failed to create patient: {Patient}", Patient);
            throw new BusinessException(ApiCode.OperationFailed, "Failed to create patient.");
        }
    }

    /// <summary>
    /// Maps the <see cref="CreatePatientRequest"/> to a <see cref="Patient"/> entity.
    /// </summary>
    /// <param name="request">The patient creation request.</param>
    /// <returns>The mapped <see cref="Patient"/> entity.</returns>
    private Patient MapToEntity(CreatePatientRequest request)
    {
        var userId = request.UserId!.Trim();
        var patient = new Patient
        {
            Name = request.Name!.Trim(),
            Age = request.Age!.Value,
            Gender = request.Gender!.Value,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
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
}
