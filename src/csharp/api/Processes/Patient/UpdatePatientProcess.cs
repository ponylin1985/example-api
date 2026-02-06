using Example.Api.Dtos.Requests;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Example.Api.Repositories;

namespace Example.Api.Processes;

/// <summary>
/// Process for updating a patient.
/// </summary>
public sealed class UpdatePatientProcess
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<UpdatePatientProcess> _logger;

    /// <summary>
    /// The request DTO containing patient update data.
    /// </summary>
    private readonly UpdatePatientRequest _request;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// The patient entity prepared for update.
    /// </summary>
    /// <value></value>
    public Patient? Patient { get; private set; }

    /// <summary>
    /// The patient entity after successful update.
    /// </summary>
    /// <value></value>
    public Patient? UpdatedPatient { get; private set; }

    /// <summary>
    /// Constructor for UpdatePatientProcess.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="request"></param>
    /// <param name="patientRepository"></param>
    public UpdatePatientProcess(
        ILogger<UpdatePatientProcess> logger,
        UpdatePatientRequest request,
        IPatientRepository patientRepository)
    {
        _logger = logger;
        _request = request;
        _patientRepository = patientRepository;
    }

    /// <summary>
    /// Prepares the patient entity from the request data.
    /// </summary>
    public UpdatePatientProcess Prepare()
    {
        Patient = MapToEntity(_request);
        return this;
    }

    /// <summary>
    /// Executes the process to update the patient in the repository.
    /// </summary>
    public async Task<UpdatePatientProcess> ExecuteAsync(IUnitOfWork unitOfWork)
    {
        UpdatedPatient = await _patientRepository.UpdateAsync(Patient!);
        await unitOfWork.SaveChangesAsync();
        return this;
    }

    /// <summary>
    /// Ensures the patient was updated successfully.
    /// </summary>
    public void ShouldSuccessfully()
    {
        if (UpdatedPatient is not { Id: > 0 })
        {
            _logger.LogError("Failed to update patient: {PatientId}", Patient?.Id);
            throw new BusinessException(ApiCode.OperationFailed, "Failed to update patient.");
        }
    }

    /// <summary>
    /// Maps UpdatePatientRequest to Patient entity.
    /// </summary>
    /// <param name="request">Update patient request DTO.</param>
    /// <returns>Patient entity.</returns>
    private Patient MapToEntity(UpdatePatientRequest request)
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
