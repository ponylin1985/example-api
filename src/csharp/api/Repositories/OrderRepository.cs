using Dapper;
using Example.Api.Data;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository for managing Orders.
/// </summary>
public sealed class OrderRepository : IOrderRepository
{
    /// <summary>
    /// Database session for database operations.
    /// </summary>
    private readonly IDbSession _dbSession;

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
        _dbSession = dbSession;
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc />
    public async Task<PatientOrder?> GetOrderAsync(long id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Prescriptions)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <inheritdoc />
    public async Task<PatientOrder> AddAsync(PatientOrder order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }

    /// <inheritdoc />
    public async Task<PatientOrder> UpdateMessageAsync(PatientOrder order)
    {
        var existingOrder = await _dbContext.Orders.FindAsync(order.Id)
            ?? throw new InvalidOperationException($"OrderId {order.Id} not found.");
        existingOrder.Instructions = order.Instructions;
        return existingOrder;
    }

    /// <inheritdoc />
    public async Task<PatientOrder?> UpdateAsync(long id, string message, DateTimeOffset updatedAt)
    {
        var sql = @"
            UPDATE ""order""
            SET message = @Message,
                updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING patient_id, created_at; ";

        var conn = await _dbSession.GetOpenConnectionAsync();
        var trans = await _dbSession.EnsureTransactionAsync();

        var result = await conn.QueryFirstOrDefaultAsync(sql, new
        {
            Message = message,
            UpdatedAt = updatedAt,
            Id = id,
        }, trans);

        if (result is null)
        {
            return default;
        }

        return new PatientOrder
        {
            Id = id,
            Instructions = message,
            PatientId = (long)result.patient_id,
            CreatedAt = (DateTimeOffset)result.created_at,
            UpdatedAt = updatedAt,
        };
    }
}
