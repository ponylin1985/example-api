using Example.Api.Data;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository for managing Orders.
/// </summary>
public class OrderRepository : IOrderRepository
{
    /// <summary>
    /// DbContext for database operations.
    /// </summary>
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="dbSession"></param>
    public OrderRepository(IDbSession dbSession)
    {
        _dbContext = dbSession.Context as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <summary>
    /// Get patient's order by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Order> GetOrderAsync(long id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .FirstAsync(o => o.Id == id);
    }

    /// <summary>
    /// Creates a new patient's order.
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public async Task<Order> CreateOrderAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }

    /// <summary>
    /// Update the message of an existing order by id.
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="message"></param>
    /// <param name="updatedAt"></param>
    /// <returns></returns>
    public async Task<Order> UpdateMessageAsync(long orderId, string message, DateTimeOffset updatedAt)
    {
        var existingOrder = await _dbContext.Orders.FindAsync(orderId);

        if (existingOrder is null)
        {
            throw new Exception($"Order with ID {orderId} not found.");
        }

        var updatedOrder = existingOrder with
        {
            Message = message,
            UpdatedAt = updatedAt,
        };

        _dbContext.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
        return updatedOrder;
    }
}
