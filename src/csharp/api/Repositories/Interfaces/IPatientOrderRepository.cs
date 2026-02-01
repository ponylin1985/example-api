using Example.Api.Enums;
using Example.Api.Models;

namespace Example.Api.Repositories;

/// <summary>
/// Repository interface for managing patient orders.
/// </summary>
public interface IPatientOrderRepository
{
    /// <summary>
    /// Retrieves patient orders with pagination and optional filtering.
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="patientId"></param>
    /// <param name="orderType"></param>
    /// <param name="orderStatus"></param>
    /// <returns></returns>
    Task<(IEnumerable<PatientOrder> Data, long TotalCount)> GetPatientOrdersAsync(
        int pageNumber,
        int pageSize,
        long? patientId = default,
        OrderType? orderType = default,
        OrderStatus? orderStatus = default);

    /// <summary>
    /// Get patient's order by id.
    /// </summary>
    /// <param name="id">The order id.</param>
    /// <returns>The order entity.</returns>
    Task<PatientOrder?> GetPatientOrderAsync(long id);

    /// <summary>
    /// Creates a new patient's order.
    /// </summary>
    /// <param name="order">The order entity to create.</param>
    /// <returns>The created order entity.</returns>
    Task<PatientOrder> AddAsync(PatientOrder order);

    /// <summary>
    /// Update the instructions of an existing order by id.
    /// </summary>
    /// <param name="order">The order entity with updated instructions.</param>
    /// <returns>The updated order entity.</returns>
    Task<PatientOrder> UpdateInstructionsAsync(PatientOrder order);

    /// <summary>
    /// Update the instructions of an order.
    /// </summary>
    /// <param name="id">The id of the order to update.</param>
    /// <param name="instructions">The new instructions for the order.</param>
    /// <param name="userId">The ID of the user making the update.</param>
    /// <param name="updatedAt">The timestamp of the update.</param>
    /// <returns>The updated order entity, or null if not found.</returns>
    Task<PatientOrder?> UpdateAsync(long id, string instructions, string userId, DateTimeOffset updatedAt);
}
