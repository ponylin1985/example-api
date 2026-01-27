using Example.Api.DateTimeOffsetProviders;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Data;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// The date time offset provider.
    /// </summary>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTimeOffsetProvider dateTimeOffsetProvider) : base(options)
    {
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <summary>
    /// Gets or sets the DbSet for Patients.
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = default!;

    /// <summary>
    /// Gets or sets the DbSet for Orders.
    /// </summary>
    public DbSet<PatientOrder> Orders { get; set; } = default!;

    /// <summary>
    /// Gets or sets the DbSet for Prescriptions.
    /// </summary>
    /// <value></value>
    public DbSet<Prescription> Prescriptions { get; set; } = default!;

    /// <summary>
    /// Gets or sets the DbSet for Prescriptions.
    /// </summary>
    /// <value></value>
    public DbSet<Medication> Medications { get; set; } = default!;

    /// <summary>
    /// Configures the schema needed for the application context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DbConfigurations.PatientDbConfiguration());
        modelBuilder.ApplyConfiguration(new DbConfigurations.PatientOrderDbConfiguration());
        modelBuilder.ApplyConfiguration(new DbConfigurations.MedicationDbConfiguration());
        modelBuilder.ApplyConfiguration(new DbConfigurations.PrescriptionDbConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entries =
            ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var utcNow = _dateTimeOffsetProvider.UtcNow;

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = utcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
            }
        }

        return base.SaveChangesAsync(ct);
    }
}
