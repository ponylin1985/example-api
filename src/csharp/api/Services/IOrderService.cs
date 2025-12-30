using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;

namespace Example.Api.Services;

/// <summary>
/// Service interface for managing orders.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Retrieves an order by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order DTO.</returns>
    Task<ApiResult<OrderDto>> GetOrderAsync(long id);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">The creation request.</param>
    /// <returns>The created order DTO.</returns>
    Task<ApiResult<OrderDto>> CreateOrderAsync(CreateOrderRequest request);

    /// <summary>
    /// Updates the message of an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="message">The new message.</param>
    /// <returns>The updated order DTO.</returns>
    Task<ApiResult<OrderDto>> UpdateMessageAsync(long id, string message);
}
