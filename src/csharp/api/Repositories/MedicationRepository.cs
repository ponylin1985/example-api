using Dapper;
using Example.Api.Data;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository for managing medications.
/// </summary>
public sealed class MedicationRepository : IMedicationRepository
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
    /// Constructor for MedicationRepository.
    /// </summary>
    /// <param name="dbSession">The database session.</param>
    public MedicationRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc/>
    public async Task<(IEnumerable<Medication> Data, long TotalCount)> GetMedicationsAsync(
        bool? isEnabled = default,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = _dbContext.Medications.AsNoTracking();

        if (isEnabled.HasValue)
        {
            query = query.Where(m => m.IsEnabled == isEnabled.Value);
        }

        var totalCount = await query.LongCountAsync();
        var data = await query
            .OrderBy(m => m.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (Data: data, TotalCount: totalCount);
    }

    /// <inheritdoc/>
    public async Task<Medication?> GetMedicationAsync(long id)
    {
        return await _dbContext
            .Medications
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<long>> GetExistingMedicationIdsAsync(
        IEnumerable<long> medicationIds,
        bool? isEnabled = default)
    {
        var sql = @"
            SELECT id
            FROM medication
            WHERE Id = ANY(@Ids) ";

        var parameters = new DynamicParameters();
        parameters.Add("Ids", medicationIds.ToArray());

        if (isEnabled.HasValue)
        {
            sql += " AND is_enabled = @IsEnabled ";
            parameters.Add("IsEnabled", isEnabled.Value);
        }

        var connection = await _dbSession.GetOpenConnectionAsync();
        return await connection.QueryAsync<long>(sql, parameters);
    }
}
