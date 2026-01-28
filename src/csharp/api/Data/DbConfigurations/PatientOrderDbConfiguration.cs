using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Example.Api.Data.DbConfigurations;

/// <summary>
/// Configures the PatientOrder entity for the database.
/// </summary>
public class PatientOrderDbConfiguration : IEntityTypeConfiguration<PatientOrder>
{
    /// <summary>
    /// Configures the PatientOrder entity.
    /// </summary>
    public void Configure(EntityTypeBuilder<PatientOrder> entity)
    {
        var emptyStringToNullConverter = new ValueConverter<string?, string?>(
            v => string.IsNullOrWhiteSpace(v) ? null : v,
            v => v
        );

        entity
            .ToTable("patient_order")
            .HasKey(o => o.Id);

        entity
            .Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        entity
            .Property(o => o.Instructions)
            .HasColumnName("instructions")
            .HasConversion(emptyStringToNullConverter)
            .HasMaxLength(500);

        entity
            .Property(o => o.NextVisitDate)
            .HasColumnName("next_visit_date")
            .HasColumnType("timestamptz");

        entity
            .Property(o => o.StartDate)
            .HasColumnName("start_date")
            .HasColumnType("timestamptz");

        entity
            .Property(o => o.EndDate)
            .HasColumnName("end_date")
            .HasColumnType("timestamptz");

        entity
            .Property(o => o.Type)
            .HasColumnName("type")
            .HasConversion<byte>()
            .HasColumnType("smallint")
            .IsRequired();

        entity
            .Property(o => o.Status)
            .HasColumnName("status")
            .HasConversion<byte>()
            .HasColumnType("smallint")
            .IsRequired();

        entity
            .Property(o => o.DispensedDate)
            .HasColumnName("dispensed_date")
            .HasColumnType("timestamptz");

        entity
            .Property(o => o.PatientId)
            .HasColumnName("patient_id")
            .HasColumnType("bigint")
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
            .HasIndex(o => o.PatientId)
            .IncludeProperties(o => new
            {
                o.Status,
                o.Type,
            })
            .HasDatabaseName("ix_patientorder_patientid");

        entity
            .HasIndex(o => new
            {
                o.Type,
                o.Status,
                o.CreatedBy,
                o.CreatedAt,
            })
            .HasDatabaseName("ix_patientorder_type_status_createdby_createdat");

        entity
            .HasOne(o => o.Patient)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasMany(o => o.Prescriptions)
            .WithOne()
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
