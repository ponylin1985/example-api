using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .HasDefaultValue(string.Empty)
            .HasMaxLength(500)
            .IsRequired();

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
            .HasIndex(o => o.PatientId)
            .IncludeProperties(o => new 
            {
                o.Instructions,
                o.Status, 
                o.Type,
            })
            .HasDatabaseName("ix_patientorder_patientid");

        entity
            .HasIndex(o => o.Type)
            .IncludeProperties(o => new 
            {
                o.PatientId,
                o.Instructions,
                o.Status,
            })
            .HasDatabaseName("ix_patientorder_type");

        entity
            .HasIndex(o => o.Status)
            .IncludeProperties(o => new 
            {
                o.PatientId,
                o.Instructions,
                o.Type,
            })
            .HasDatabaseName("ix_patientorder_status");

        entity
            .HasIndex(o => new 
            { 
                o.Status, 
                o.Type,
            })
            .IncludeProperties(o => new 
            {
                o.PatientId,
                o.Instructions,
            })
            .HasDatabaseName("ix_patientorder_status_type");

        entity
            .HasIndex(o => new 
            { 
                o.CreatedBy, 
                o.CreatedAt,
            })
            .IncludeProperties(o => new 
            {
                o.PatientId,
                o.Instructions,
                o.Status, 
                o.Type,
            })
            .HasDatabaseName("ix_patientorder_createdby_createdat");

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
