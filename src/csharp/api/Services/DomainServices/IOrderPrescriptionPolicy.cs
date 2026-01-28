using Example.Api.Models;

namespace Example.Api.Services.DomainServices;

/// <summary>
/// Interface for order prescription policy validations.
/// </summary>
public interface IOrderPrescriptionPolicy
{   
    /// <summary>
    /// Ensures that all medication IDs in the order are valid.
    /// </summary>
    /// <param name="order">The patient order to validate.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EnsureMedicationIdsValidAsync(PatientOrder order);
}
