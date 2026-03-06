using Example.Api.Data;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Repositories;

/// <summary>
/// Data repository for managing patients.
/// </summary>
public sealed class PatientRepository : IPatientRepository
{
    /// <summary>
    /// DbContext for database operations.
    /// </summary>
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class.
    /// </summary>
    /// <param name="dbSession">The database session.</param>
    public PatientRepository(IDbSession dbSession)
    {
        _dbContext = dbSession.DataContext as ApplicationDbContext
            ?? throw new ArgumentException("Invalid DbContext type in DbSession.");
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<Patient> Data, long TotalCount)> GetPatientsAsync(
        int pageNumber,
        int pageSize,
        string? name = default,
        bool isPrefix = false,
        string? email = default,
        string? phoneNumber = default,
        PatientStatus? status = default,
        DateTimeOffset? startTime = default,
        DateTimeOffset? endTime = default)
    {
        var query = _dbContext.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(p => p.Email == email && p.Email != null);
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            query = query.Where(p => p.PhoneNumber == phoneNumber && p.PhoneNumber != null);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = isPrefix ?
                query.Where(p => p.Name.StartsWith(name)) :
                query.Where(p => p.Name == name);
        }

        if (startTime.HasValue && endTime.HasValue)
        {
            query = query.Where(p => p.CreatedAt >= startTime && p.CreatedAt <= endTime);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var totalCount = await query.LongCountAsync();
        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Orders.OrderByDescending(o => o.CreatedAt))
            .ThenInclude(o => o.Prescriptions.OrderByDescending(s => s.Id))
            .AsSplitQuery()
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
    public async Task<bool> IsExistPatientByEmailAsync(string email)
    {
        return await _dbContext
            .Patients
            .AsNoTracking()
            .AnyAsync(p => p.Email == email);
    }

    /// <inheritdoc />
    public async Task<bool> IsExistPatientByPhoneAsync(string phoneNumber)
    {
        return await _dbContext
            .Patients
            .AsNoTracking()
            .AnyAsync(p => p.PhoneNumber == phoneNumber);
    }

    /// <inheritdoc />
    public async Task<Patient?> GetPatientAsync(long id)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .Include(p => p.Orders.OrderByDescending(o => o.Id))
            .ThenInclude(o => o.Prescriptions.OrderByDescending(pr => pr.Id))
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<Patient?> GetPatientByNameAsync(string name)
    {
        return await _dbContext.Patients
            .AsNoTracking()
            .Include(p => p.Orders.OrderByDescending(o => o.Id))
            .ThenInclude(o => o.Prescriptions.OrderByDescending(pr => pr.Id))
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    /// <inheritdoc />
    public async Task<Patient> AddAsync(Patient patient)
    {
        await _dbContext.Patients.AddAsync(patient);
        return patient;
    }

    public async Task<Patient> UpdateAsync(Patient patient)
    {
        var existingPatient = await _dbContext.Patients.FindAsync(patient.Id)
            ?? throw new BusinessException(ApiCode.NoDataFound, $"PatientId {patient.Id} not found.");
        existingPatient.Name = patient.Name;
        existingPatient.Age = patient.Age;
        existingPatient.Gender = patient.Gender;
        existingPatient.Email = patient.Email;
        existingPatient.PhoneNumber = patient.PhoneNumber;
        existingPatient.DateOfBirth = patient.DateOfBirth;
        existingPatient.Address = patient.Address;
        existingPatient.Remarks = patient.Remarks;
        existingPatient.UpdatedBy = patient.UpdatedBy;
        existingPatient.UpdatedAt = patient.UpdatedAt;
        return existingPatient;
    }
}
