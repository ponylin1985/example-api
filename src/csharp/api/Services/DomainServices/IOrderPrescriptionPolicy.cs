namespace Example.Api.Services.DomainServices;

/// <summary>
/// Interface for order prescription policy validations.
/// </summary>
public interface IOrderPrescriptionPolicy
{
    /// <summary>
    /// Ensures that the medication IDs in the prescriptions are valid.
    /// </summary>
    /// <param name="requestedMedicationIds">Requested medicationIds.</param>
    /// <param name="existingMedicationIds">Existing medicationIds.</param>
    void EnsureMedicationIdsValid(
        IEnumerable<long> requestedMedicationIds,
        IEnumerable<long> existingMedicationIds);
}
