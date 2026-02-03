using Example.Api.Data;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

public sealed class PatientOrderHistoryRepository : IPatientOrderHistoryRepository
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
    /// Initializes a new instance of the <see cref="PatientOrderHistoryRepository"/> class.
    /// </summary>
    /// <param name="dbSession"></param>
    public PatientOrderHistoryRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<PatientOrderHistory> Data, long TotalCount)> GetHistoriesByPatientIdAsync(
        long patientId,
        int pageNumber,
        int pageSize)
    {
        var query = _dbContext.OrderHistories
            .AsNoTracking()
            .Where(oh => oh.PatientId == patientId);

        var totalCount = await query.LongCountAsync();
        var data = await query
            .OrderByDescending(oh => oh.OrderId)
            .ThenByDescending(oh => oh.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<PatientOrderHistory> Data, long TotalCount)> GetHistoriesByOrderIdAsync(
        long orderId,
        int pageNumber,
        int pageSize)
    {
        var query = _dbContext.OrderHistories
            .AsNoTracking()
            .Where(oh => oh.OrderId == orderId);

        var totalCount = await query.LongCountAsync();
        var data = await query
            .OrderByDescending(oh => oh.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    /// <inheritdoc />
    public async Task<PatientOrderHistory> AddHistoryAsync(PatientOrderHistory orderHistory)
    {
        await _dbContext.OrderHistories.AddAsync(orderHistory);
        return orderHistory;
    }
}
