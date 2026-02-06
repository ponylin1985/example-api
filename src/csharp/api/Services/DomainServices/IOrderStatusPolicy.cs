using Example.Api.Models;

namespace Example.Api.Services.DomainServices;

/// <summary>
/// Interface for order status policy validations.
/// </summary>
public interface IOrderStatusPolicy
{
    /// <summary>
    /// Ensure that the patient order exists.
    /// </summary>
    /// <param name="patientOrder">The patient order.</param>
    /// <returns></returns>
    IOrderStatusPolicy EnsurePatientOrderExists(PatientOrder? patientOrder);

    /// <summary>
    /// Ensures that the patient order can be dispensed.
    /// </summary>
    void EnsureCanBeDispensed();

    /// <summary>
    /// Ensures that the patient order can be executed.
    /// </summary>
    void EnsureCanBeExecuted();

    /// <summary>
    /// Ensures that the patient order can be cancelled.
    /// </summary>
    void EnsureCanBeCancelled();

    /// <summary>
    /// Checks if the patient order can be dispensed.
    /// </summary>
    bool CanBeDispensed();

    /// <summary>
    /// Checks if the patient order can be executed.
    /// </summary>
    bool CanBeExecuted();

    /// <summary>
    /// Checks if the patient order can be cancelled.
    /// </summary>
    bool CanBeCancelled();
}
