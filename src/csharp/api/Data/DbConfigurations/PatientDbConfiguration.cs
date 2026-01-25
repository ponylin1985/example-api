using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Example.Api.Data.DbConfigurations;

/// <summary>
/// Configures the Patient entity for the database.
/// </summary>
public class PatientDbConfiguration : IEntityTypeConfiguration<Patient>
{
    /// <summary>
    /// Configures the Patient entity.
    /// </summary>
    /// <param name="entity"></param>
    public void Configure(EntityTypeBuilder<Patient> entity)
    {
        entity
            .ToTable("patient")
            .HasKey(p => p.Id)
            .HasName("pk_patient_id");

        entity
            .Property(p => p.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        entity
            .Property(p => p.Name)
            .HasColumnName("name")
            .HasDefaultValue(string.Empty)
            .IsRequired()
            .HasMaxLength(50);

        entity
            .Property(p => p.Age)
            .HasColumnName("age")
            .IsRequired();

        entity
            .Property(p => p.Gender)
            .HasColumnName("gender")
            .HasConversion<int>()
            .IsRequired();

        entity
            .Property(p => p.Email)
            .HasColumnName("email")
            .HasDefaultValue(string.Empty)
            .HasMaxLength(100)
            .IsRequired();

        entity
            .Property(p => p.PhoneNumber)
            .HasColumnName("phone_number")
            .HasDefaultValue(string.Empty)
            .HasMaxLength(10)
            .IsRequired();

        entity
            .OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Country).HasColumnName("country").HasDefaultValue(string.Empty).HasMaxLength(25);
                address.Property(a => a.City).HasColumnName("city").HasDefaultValue(string.Empty).HasMaxLength(25);
                address.Property(a => a.Area).HasColumnName("area").HasDefaultValue(string.Empty).HasMaxLength(25);
                address.Property(a => a.Road).HasColumnName("road").HasDefaultValue(string.Empty).HasMaxLength(25);
                address.Property(a => a.Street).HasColumnName("street").HasDefaultValue(string.Empty).HasMaxLength(25);
                address.Property(a => a.Others).HasColumnName("address_others").HasDefaultValue(string.Empty).HasMaxLength(100);
            });

        entity
            .Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth")
            .HasColumnType("timestamptz")
            .IsRequired();

        entity
            .Property(p => p.FirstVisitDate)
            .HasColumnName("first_visit_date")
            .HasColumnType("timestamptz")
            .IsRequired();

        entity
            .Property(p => p.Status)
            .HasColumnName("status")
            .HasConversion<byte>()
            .HasColumnType("smallint")
            .IsRequired();

        entity
            .Property(p => p.Remarks)
            .HasColumnName("remarks")
            .HasMaxLength(500);

        entity
            .Property(p => p.CreatedBy)
            .HasColumnName("created_by")
            .HasDefaultValue(string.Empty)
            .HasMaxLength(50)
            .IsRequired();

        entity
            .Property(p => p.CreatedAt)
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
            .Property(p => p.UpdatedAt)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .HasColumnName("updated_at")
            .IsRequired();

        entity
            .HasIndex(p => p.Email)
            .IsUnique();

        entity
            .HasIndex(p => p.PhoneNumber)
            .IsUnique();

        entity
            .HasIndex(p => new
            {
                p.Name,
                p.Email,
                p.PhoneNumber,
            })
            .IncludeProperties(p => new
            {
                p.Age,
                p.Gender,
                p.Status,
            })
            .HasDatabaseName("ix_patient_name_email_phoneNumber");

        entity
            .HasIndex(p => new
            {
                p.CreatedBy,
                p.CreatedAt
            })
            .IncludeProperties(p => new
            {
                p.Age,
                p.Gender,
                p.Status,
                p.Name,
                p.Email,
                p.PhoneNumber,
            })
            .HasDatabaseName("ix_patient_createdby_createdat");

        entity
            .HasMany(p => p.Orders)
            .WithOne(o => o.Patient)
            .HasForeignKey(o => o.PatientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
