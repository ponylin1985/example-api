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
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Order> GetOrderAsync(long id);

    /// <summary>
    /// Creates a new patient's order.
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    Task<Order> CreateOrderAsync(Order order);

    /// <summary>
    /// Update the message of an existing order by id.
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="message"></param>
    /// <param name="updatedAt"></param>
    /// <returns></returns>
    Task<Order> UpdateMessageAsync(long orderId, string message, DateTimeOffset updatedAt);
}
