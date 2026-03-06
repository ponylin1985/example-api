using Example.Api.Models;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository interface for managing medications.
/// </summary>
public interface IMedicationRepository
{
    /// <summary>
    /// Retrieves all medications with optional filtering and pagination.
    /// </summary>
    /// <param name="isEnabled">Optional filter for enabled medications.</param>
    /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
    /// <param name="pageSize">The number of items per page. Default is 10.</param>
    /// <returns>A collection of medications and the total count.</returns>
    Task<(IReadOnlyList<Medication> Data, long TotalCount)> GetMedicationsAsync(
        bool? isEnabled = default,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// Checks if a medication exists by its ID.
    /// </summary>
    /// <param name="id">The id of the medication.</param>
    /// <returns>The medication if it exists, otherwise null.</returns>
    Task<Medication?> GetMedicationAsync(long id);

    /// <summary>
    /// Gets the medicationIds by the given medicationIds.
    /// </summary>
    /// <param name="medicationIds">Request medicationIds.</param>
    /// <param name="isEnabled">Optional filter for enabled medications.</param>
    /// <returns>MedicationIds that exist in the database.</returns>
    Task<IReadOnlyList<long>> GetExistingMedicationIdsAsync(
        IEnumerable<long> medicationIds,
        bool? isEnabled = default);
}
