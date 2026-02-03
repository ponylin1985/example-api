using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;

namespace Example.Api.Services;

/// <summary>
/// Service interface for managing patient orders.
/// </summary>
public interface IPatientOrderService
{
    /// <summary>
    /// Retrieves a paginated list of patient orders based on the provided request parameters.
    /// </summary>
    /// <param name="request">The request containing pagination and filter parameters.</param>
    /// <returns>A paginated list of patient orders.</returns>
    Task<ApiResult<PagedResult<PatientOrderDto>>> GetPatientOrdersAsync(GetPatientOrdersRequest request);

    /// <summary>
    /// Retrieves an order by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> GetPatientOrderAsync(long id);

    /// <summary>
    /// Retrieves order history for a specific patient.
    /// </summary>
    /// <param name="patientId">The unique identifier of the patient.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of patient order history DTOs.</returns>
    Task<ApiResult<PagedResult<PatientOrderHistoryDto>>> GetOrderHistoryByPatientIdAsync(
        long patientId,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Retrieves order history for a specific order.
    /// </summary>
    /// <param name="orderId">The unique identifier of the order.</param>
    /// <param name="pageNumber">The page number for pagination.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of patient order history DTOs.</returns>
    Task<ApiResult<PagedResult<PatientOrderHistoryDto>>> GetOrderHistoryByOrderIdAsync(
        long orderId,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">The creation request.</param>
    /// <returns>The created patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> AddPatientOrderAsync(CreatePatientOrderRequest request);

    /// <summary>
    /// Dispenses an existing order.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <returns>The updated patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> DispenseOrderAsync(UpdatePatientOrderRequest request);

    /// <summary>
    /// Executes an existing order.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <returns>The updated patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> ExecuteOrderAsync(UpdatePatientOrderRequest request);

    /// <summary>
    /// Cancels an existing order.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <returns>The updated patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> CancelOrderAsync(UpdatePatientOrderRequest request);
}
