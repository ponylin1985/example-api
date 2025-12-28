using Example.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Data;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for Patients.
    /// </summary>
    public DbSet<Patient> Patients { get; set; } = default!;

    /// <summary>
    /// Gets or sets the DbSet for Orders.
    /// </summary>
    public DbSet<Order> Orders { get; set; } = default!;

    /// <summary>
    /// Configures the schema needed for the application context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity
                .ToTable("patient")
                .HasKey(p => p.Id);

            entity
                .Property(p => p.Id)
                .HasColumnType("bigint")
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity
                .Property(p => p.Name)
                .HasColumnName("name")
                .HasDefaultValue(string.Empty)
                .IsRequired()
                .HasMaxLength(50);

            entity
                .Property(p => p.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .HasColumnName("created_at")
                .IsRequired();

            entity
                .Property(p => p.UpdatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .HasColumnName("updated_at")
                .IsRequired();

            entity
                .HasIndex(p => p.Name);

            entity
                .HasMany(p => p.Orders)
                .WithOne(o => o.Patient)
                .HasForeignKey(o => o.PatientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity
                .ToTable("order")
                .HasKey(o => o.Id);

            entity
                .Property(o => o.Id)
                .HasColumnType("bigint")
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity
                .Property(o => o.Message)
                .HasColumnName("message")
                .IsRequired()
                .HasMaxLength(500);

            entity
                .Property(o => o.PatientId)
                .HasColumnType("bigint")
                .HasColumnName("patient_id");

            entity
                .Property(p => p.CreatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .HasColumnName("created_at")
                .IsRequired();

            entity
                .Property(p => p.UpdatedAt)
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
                .HasColumnName("updated_at")
                .IsRequired();

            entity
                .HasIndex(o => o.PatientId)
                .HasDatabaseName("IX_Order_PatientId");
        });

        base.OnModelCreating(modelBuilder);
    }
}
