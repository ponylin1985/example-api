using Example.Api.Models;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository interface for managing medications.
/// </summary>
public interface IMedicationRepository
{
    /// <summary>
    /// Retrieves all medications.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Medication>> GetMedicationsAsync();

    /// <summary>
    /// Checks if a medication exists by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> IsExistMedicationAsync(long id);
}
