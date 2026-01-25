using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        entity.ToTable("prescription");
        entity.HasKey(p => p.Id);

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
            .HasDefaultValue(string.Empty)
            .HasMaxLength(50)
            .IsRequired();

        entity.Property(p => p.Frequency)
            .HasColumnName("frequency")
            .HasDefaultValue(string.Empty)
            .HasMaxLength(50)
            .IsRequired();

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

        entity.HasOne(p => p.Medication)
            .WithMany()
            .HasForeignKey(p => p.MedicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
