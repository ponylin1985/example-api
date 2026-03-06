using Example.Api.Models;

namespace Example.Api.Repositories;

/// <summary>
/// Repository interface for managing patient order histories.
/// </summary>
public interface IPatientOrderHistoryRepository
{
    /// <summary>
    /// Retrieves order histories for a specific patient.
    /// </summary>
    /// <param name="patientId">The patient identifier.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The page size for pagination.</param>
    /// <returns>A collection of patient order histories.</returns>
    Task<(IReadOnlyList<PatientOrderHistory> Data, long TotalCount)> GetHistoriesByPatientIdAsync(
        long patientId,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Retrieves order histories for a specific order.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The page size for pagination.</param>
    /// <returns>A collection of patient order histories.</returns>
    Task<(IReadOnlyList<PatientOrderHistory> Data, long TotalCount)> GetHistoriesByOrderIdAsync(
        long orderId,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Adds a new patient order history.
    /// </summary>
    /// <param name="orderHistory">The patient order history to add.</param>
    /// <returns>The added patient order history.</returns>
    Task<PatientOrderHistory> AddHistoryAsync(PatientOrderHistory orderHistory);
}
