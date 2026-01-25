using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        entity.ToTable("medication");
        entity.HasKey(m => m.Id);

        entity.Property(m => m.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        entity.Property(m => m.Name)
            .HasColumnName("name")
            .HasDefaultValue(string.Empty)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(m => m.Manufacturer)
            .HasColumnName("manufacturer")
            .HasDefaultValue(string.Empty)
            .IsRequired()
            .HasMaxLength(50);

        entity
            .Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasDefaultValue(string.Empty)
            .HasMaxLength(50)
            .IsRequired();

        entity
            .Property(o => o.CreatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .HasColumnName("created_at")
            .IsRequired();

        entity
            .Property(p => p.UpdatedBy)
            .HasColumnName("updated_by")
            .HasDefaultValue(string.Empty)
            .HasMaxLength(50)
            .IsRequired();

        entity
            .Property(o => o.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .HasColumnName("updated_at")
            .IsRequired();

        entity
            .HasIndex(m => m.Name)
            .IsUnique()
            .HasDatabaseName("ix_medication_name");
    }
}
