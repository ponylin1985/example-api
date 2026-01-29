using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        var emptyStringToNullConverter = new ValueConverter<string?, string?>(
            v => string.IsNullOrWhiteSpace(v) ? null : v,
            v => v
        );

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
            .IsRequired()
            .HasMaxLength(50);

        entity
            .Property(p => p.Age)
            .HasColumnName("age")
            .IsRequired();

        entity
            .Property(p => p.Gender)
            .HasColumnName("gender")
            .HasConversion<byte>()
            .HasColumnType("smallint")
            .IsRequired();

        entity
            .Property(p => p.Email)
            .HasColumnName("email")
            .HasConversion(emptyStringToNullConverter)
            .HasMaxLength(100);

        entity
            .Property(p => p.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(10)
            .IsRequired();

        entity
            .OwnsOne(p => p.Address, address =>
            {
                address
                    .Property(a => a.Country)
                    .HasColumnName("country")
                    .HasConversion(emptyStringToNullConverter)
                    .HasMaxLength(25);
                address.Property(a => a.City)
                    .HasColumnName("city")
                    .HasConversion(emptyStringToNullConverter)
                    .HasMaxLength(25);
                address.Property(a => a.Area)
                    .HasColumnName("area")
                    .HasConversion(emptyStringToNullConverter)
                    .HasMaxLength(25);
                address.Property(a => a.Road)
                    .HasColumnName("road")
                    .HasConversion(emptyStringToNullConverter)
                    .HasMaxLength(25);
                address.Property(a => a.Street)
                    .HasColumnName("street")
                    .HasConversion(emptyStringToNullConverter)
                    .HasMaxLength(25);
                address.Property(a => a.Others)
                    .HasColumnName("address_others")
                    .HasConversion(emptyStringToNullConverter)
                    .HasMaxLength(100);
            });

        entity
            .Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth")
            .HasColumnType("date")
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
            .HasConversion(emptyStringToNullConverter)
            .HasMaxLength(500);

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
            .HasIndex(p => p.Email)
            .IsUnique()
            .HasFilter("email IS NOT NULL")
            .HasDatabaseName("ix_patient_email");

        entity
            .HasIndex(p => p.PhoneNumber)
            .IsUnique()
            .HasFilter("phone_number IS NOT NULL")
            .HasDatabaseName("ix_patient_phonenumber");

        entity
            .HasIndex(p => new
            {
                p.CreatedAt,
                p.CreatedBy,
                p.Name,
            })
            .IncludeProperties(p => new
            {
                p.Status,
            })
            .HasDatabaseName("ix_patient_createdby_createdat_name");

        entity
            .HasMany(p => p.Orders)
            .WithOne(o => o.Patient)
            .HasForeignKey(o => o.PatientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
