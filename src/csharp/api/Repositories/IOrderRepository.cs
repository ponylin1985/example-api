using Example.Api.Models;

namespace Example.Api.Repositories;

public interface IOrderRepository
{
    /// <summary>
    /// Get patient's order by id.
    /// </summary>
    /// <param name="id">The order id.</param>
    /// <returns>The order entity.</returns>
    Task<Order?> GetOrderAsync(long id);

    /// <summary>
    /// Creates a new patient's order.
    /// </summary>
    /// <param name="order">The order entity to create.</param>
    /// <returns>The created order entity.</returns>
    Task<Order> AddAsync(Order order);

    /// <summary>
    /// Update the message of an existing order by id.
    /// </summary>
    /// <param name="order">The order entity with updated message.</param>
    /// <returns>The updated order entity.</returns>
    Task<Order> UpdateMessageAsync(Order order);

    /// <summary>
    /// Update the message of an order.
    /// </summary>
    /// <param name="id">The id of the order to update.</param>
    /// <param name="message">The new message for the order.</param>
    /// <param name="updatedAt">The timestamp of the update.</param>
    /// <returns>The updated order entity, or null if not found.</returns>
    Task<Order?> UpdateAsync(long id, string message, DateTimeOffset updatedAt);
}
