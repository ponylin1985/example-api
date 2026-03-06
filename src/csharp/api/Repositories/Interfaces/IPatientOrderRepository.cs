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
    Task<(IReadOnlyList<PatientOrder> Data, long TotalCount)> GetPatientOrdersAsync(
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
    /// Updates an existing patient's order.<para/>
    /// This is a patch atomic operation with the original order status.
    /// </summary>
    /// <param name="order">The order entity with updated information.</param>
    /// <param name="originalStatus">The original status of the order before update.</param>
    /// <returns>The updated order entity, or null if not found.</returns>
    Task<PatientOrder?> PatchAsync(PatientOrder order, OrderStatus originalStatus);
}
