using Example.Api.Data;
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
    /// <param name="dbContext"></param>
    public PatientRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves patients created within the specified time range with pagination.
    /// </summary>
    /// <param name="startTime">The start time of the range.</param>
    /// <param name="endTime">The end time of the range.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A tuple containing the list of patients and the total count.</returns>
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
            .Include(p => p.Orders)
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, totalCount);
    }

    /// <summary>
    /// Checks if a patient exists by their ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> IsExistPatientAsync(long id)
    {
        return await _dbContext
            .Patients
            .AsNoTracking()
            .AnyAsync(p => p.Id == id);
    }

    /// <summary>
    /// Retrieves a patient by their ID.
    /// </summary>
    /// <param name="id">The ID of the patient.</param>
    /// <returns>The patient with the specified ID, or null if not found.</returns>
    public async Task<Patient?> GetPatientAsync(long id)
    {
        return await _dbContext
            .Patients
            .AsNoTracking()
            .Include(p => p.Orders)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Retrieves a patient by their name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<Patient?> GetPatientByNameAsync(string name)
    {
        return await _dbContext
            .Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    /// <summary>
    /// Creates a new patient record.
    /// </summary>
    /// <param name="patient"></param>
    /// <returns></returns>
    public async Task<Patient> CreatePatientAsync(Patient patient)
    {
        await _dbContext.Patients.AddAsync(patient);
        await _dbContext.SaveChangesAsync();
        return patient;
    }
}
