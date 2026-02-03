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
    public async Task<(IEnumerable<PatientOrder> Data, long TotalCount)> GetPatientOrdersAsync(
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
    public async Task<PatientOrder?> PatchAsync(PatientOrder order)
    {
        var sql = @"
            UPDATE patient_order
            SET instructions = @Instructions,
                status = @Status,
                updated_by = @UpdatedBy,
                updated_at = @UpdatedAt
            WHERE id = @Id
            RETURNING *; ";

        var conn = await _dbSession.GetOpenConnectionAsync();
        var trans = await _dbSession.EnsureTransactionAsync();

        var result = await conn.QueryFirstOrDefaultAsync(sql, new
        {
            order.Instructions,
            order.Status,
            order.UpdatedBy,
            order.UpdatedAt,
            order.Id,
        }, trans);

        var updatedOrder = result is null
            ? default
            : new PatientOrder
            {
                Id = (long)result.id,
                Instructions = (string)result.instructions,
                Type = (OrderType)result.type,
                Status = (OrderStatus)result.status,
                PatientId = (long)result.patient_id,
                UpdatedBy = (string)result.updated_by,
                UpdatedAt = (DateTimeOffset)result.updated_at,
                CreatedAt = (DateTimeOffset)result.created_at,
                CreatedBy = (string)result.created_by,
            };

        return updatedOrder;
    }
}
