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
    /// DbContext for database operations.
    /// </summary>
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Constructor for MedicationRepository.
    /// </summary>
    /// <param name="dbSession">The database session.</param>
    public MedicationRepository(IDbSession dbSession)
    {
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <summary>
    /// Retrieves all medications.
    /// </summary>
    /// <returns>A list of all medications.</returns>
    public async Task<IEnumerable<Medication>> GetMedicationsAsync()
    {
        return await _dbContext.Medications
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Checks if a medication exists by its ID.
    /// </summary>
    /// <param name="id">The ID of the medication.</param>
    /// <returns>True if the medication exists, otherwise false.</returns>
    public async Task<bool> IsExistMedicationAsync(long id)
    {
        return await _dbContext
            .Medications
            .AsNoTracking()
            .AnyAsync(m => m.Id == id);
    }
}
