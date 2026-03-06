using Dapper;
using Example.Api.Data;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository for managing PatientOrders.
/// </summary>
public sealed class PatientOrderRepository : IPatientOrderRepository
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
    /// Initializes a new instance of the <see cref="PatientOrderRepository"/> class.
    /// </summary>
    /// <param name="dbSession"></param>
    public PatientOrderRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<PatientOrder> Data, long TotalCount)> GetPatientOrdersAsync(
        int pageNumber,
        int pageSize,
        long? patientId = default,
        OrderType? orderType = default,
        OrderStatus? orderStatus = default)
    {
        var query = _dbContext.Orders
            .AsNoTracking();

        if (patientId.HasValue)
        {
            query = query.Where(o => o.PatientId == patientId.Value);
        }

        if (orderType.HasValue)
        {
            query = query.Where(o => o.Type == orderType.Value);
        }

        if (orderStatus.HasValue)
        {
            query = query.Where(o => o.Status == orderStatus.Value);
        }

        var totalCount = await query.LongCountAsync();
        var data = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(o => o.Prescriptions.OrderByDescending(p => p.Id))
            .AsSplitQuery()
            .ToListAsync();

        return (data, totalCount);
    }

    /// <inheritdoc />
    public async Task<PatientOrder?> GetPatientOrderAsync(long id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Prescriptions.OrderByDescending(p => p.Id))
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <inheritdoc />
    public async Task<PatientOrder> AddAsync(PatientOrder order)
    {
        await _dbContext.Orders.AddAsync(order);
        return order;
    }

    /// <inheritdoc />
    public async Task<PatientOrder?> PatchAsync(PatientOrder order, OrderStatus originalStatus)
    {
        var sql = @"
            UPDATE patient_order
            SET instructions = @Instructions,
                status = @Status,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
              AND status = @OriginalStatus
            RETURNING *; ";

        var conn = await _dbSession.GetOpenConnectionAsync();
        var trans = await _dbSession.EnsureTransactionAsync();

        var result = await conn.QueryFirstOrDefaultAsync<PatientOrderRecord>(sql, new
        {
            order.Instructions,
            order.Status,
            order.UpdatedBy,
            order.UpdatedAt,
            order.Id,
            OriginalStatus = originalStatus,
        }, trans);

        var updatedOrder = result is null
            ? default
            : new PatientOrder
            {
                Id = result.id,
                Instructions = result.instructions,
                Type = result.type,
                Status = result.status,
                PatientId = result.patient_id,
                UpdatedBy = result.updated_by,
                UpdatedAt = result.updated_at,
                CreatedAt = result.created_at,
                CreatedBy = result.created_by,
            };

        return updatedOrder;
    }

#pragma warning disable IDE1006 // Naming Styles
    private class PatientOrderRecord
    {
        public long id { get; init; }
        public string? instructions { get; init; }
        public OrderType type { get; init; }
        public OrderStatus status { get; init; }
        public long patient_id { get; init; }
        public required string updated_by { get; init; }
        public DateTimeOffset updated_at { get; init; }
        public DateTimeOffset created_at { get; init; }
        public required string created_by { get; init; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
