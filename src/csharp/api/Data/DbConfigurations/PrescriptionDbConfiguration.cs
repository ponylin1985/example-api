using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Example.Api.Data.DbConfigurations;

/// <summary>
/// Configures the Prescription entity for the database.
/// </summary>
public class PrescriptionDbConfiguration : IEntityTypeConfiguration<Prescription>
{
    /// <summary>
    /// Configures the Prescription entity.
    /// </summary>
    public void Configure(EntityTypeBuilder<Prescription> entity)
    {
        var emptyStringToNullConverter = new ValueConverter<string?, string?>(
            v => string.IsNullOrWhiteSpace(v) ? null : v,
            v => v
        );

        entity
            .ToTable("prescription")
            .HasKey(p => p.Id);

        entity.Property(p => p.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        entity.Property(p => p.OrderId)
            .HasColumnName("order_id")
            .HasColumnType("bigint")
            .IsRequired();

        entity.Property(p => p.MedicationId)
            .HasColumnName("medication_id")
            .HasColumnType("bigint")
            .IsRequired();

        entity.Property(p => p.Dose)
            .HasColumnName("dose")
            .HasConversion(emptyStringToNullConverter)
            .HasMaxLength(50);

        entity.Property(p => p.Frequency)
            .HasColumnName("frequency")
            .HasConversion(emptyStringToNullConverter)
            .HasMaxLength(50);

        entity.Property(p => p.DurationInDays)
            .HasColumnName("duration_in_days")
            .IsRequired();

        entity.Property(p => p.Route)
            .HasColumnName("route")
            .HasConversion<byte>()
            .HasColumnType("smallint")
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
            .HasIndex(p => p.OrderId)
            .HasDatabaseName("ix_prescription_order_id");

        entity
            .HasIndex(p => p.MedicationId)
            .HasDatabaseName("ix_prescription_medication_id");

        entity.HasOne(p => p.Medication)
            .WithMany()
            .HasForeignKey(p => p.MedicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
