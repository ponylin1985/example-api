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
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc />
    public async Task<Order> GetOrderAsync(long id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .FirstAsync(o => o.Id == id);
    }

    /// <inheritdoc />
    public async Task<Order> CreateOrderAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }

    /// <inheritdoc />
    public async Task<Order> UpdateMessageAsync(Order order)
    {
        var existingOrder = await _dbContext.Orders.FindAsync(order.Id)
            ?? throw new Exception($"OrderId {order.Id} not found.");

        var updatedOrder = existingOrder with
        {
            Message = order.Message,
            UpdatedAt = order.UpdatedAt,
        };

        _dbContext.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);
        return updatedOrder;
    }
}
