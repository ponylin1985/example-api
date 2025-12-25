using Example.Api.Data;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository for managing patients.
/// </summary>
public class PatientRepository : IPatientRepository
{
    /// <summary>
    /// DbContext for database operations.
    /// </summary>
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class.
    /// </summary>
    /// <param name="dbSession"></param>
    public PatientRepository(IDbSession dbSession)
    {
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Patient> Data, long TotalCount)> GetPatientsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int pageNumber,
        int pageSize)
    {
        var query = _dbContext.Patients
            .AsNoTracking()
            .Where(p => p.CreatedAt >= startTime && p.CreatedAt <= endTime);

        var totalCount = await query.LongCountAsync();

        var data = await query
            .Include(p => p.Orders.OrderByDescending(o => o.Id))
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    /// <inheritdoc />
    public async Task<bool> IsExistPatientAsync(long id)
    {
        return await _dbContext
            .Patients
            .AsNoTracking()
            .AnyAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<Patient?> GetPatientAsync(long id)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .Include(p => p.Orders.OrderByDescending(o => o.Id))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<Patient?> GetPatientByNameAsync(string name)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .Include(p => p.Orders.OrderByDescending(o => o.Id))
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    /// <inheritdoc />
    public async Task<Patient> CreatePatientAsync(Patient patient)
    {
        await _dbContext.Patients.AddAsync(patient);
        return patient;
    }
}
