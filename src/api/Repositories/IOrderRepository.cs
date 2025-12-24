using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    Task<Order> CreateOrderAsync(Order order);

    /// <summary>
    /// Update the message of an existing order by id.
    /// </summary>
    /// <param name="order">The order entity with updated message.</param>
    /// <returns>The updated order entity.</returns>
    Task<Order> UpdateMessageAsync(Order order);
}
