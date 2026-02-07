using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Example.Api.Data.DbConfigurations;

/// <summary>
/// Configures the Medication entity for the database.
/// </summary>
public class MedicationDbConfiguration : IEntityTypeConfiguration<Medication>
{
    /// <summary>
    /// Configures the Medication entity.
    /// </summary>
    public void Configure(EntityTypeBuilder<Medication> entity)
    {
        var emptyStringToNullConverter = new ValueConverter<string?, string?>(
            v => string.IsNullOrWhiteSpace(v) ? null : v,
            v => v
        );

        entity
            .ToTable("medication")
            .HasKey(m => m.Id);

        entity
            .Property(m => m.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        entity
            .Property(m => m.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50);

        entity
            .Property(m => m.Manufacturer)
            .HasColumnName("manufacturer")
            .IsRequired()
            .HasMaxLength(50);

        entity
            .Property(m => m.IsEnabled)
            .HasColumnName("is_enabled")
            .HasColumnType("boolean")
            .HasDefaultValue(true)
            .IsRequired();

        entity
            .Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(50)
            .IsRequired();

        entity
            .Property(p => p.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("TIMEZONE('utc', NOW())")
            .HasColumnName("created_at")
            .IsRequired();

        entity
            .Property(p => p.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(50)
            .IsRequired();

        entity
            .Property(p => p.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("TIMEZONE('utc', NOW())")
            .HasColumnName("updated_at")
            .IsRequired();

        entity
            .HasIndex(m => m.Name)
            .IsUnique()
            .HasFilter("name IS NOT NULL")
            .HasDatabaseName("ix_medication_name");
    }
}
