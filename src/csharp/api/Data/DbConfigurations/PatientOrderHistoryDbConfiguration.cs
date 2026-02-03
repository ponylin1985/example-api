using Example.Api.Enums;
using Example.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Example.Api.Data.DbConfigurations;

/// <summary>
/// EF Core configuration for PatientOrderHistory entity.
/// </summary>
public class PatientOrderHistoryDbConfiguration : IEntityTypeConfiguration<PatientOrderHistory>
{
    /// <summary>
    /// Configures the PatientOrderHistory entity.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<PatientOrderHistory> builder)
    {
        var emptyStringToNullConverter = new ValueConverter<string?, string?>(
            v => string.IsNullOrWhiteSpace(v) ? null : v,
            v => v
        );

        builder
            .ToTable("patient_order_history")
            .HasKey(p => p.Id);

        builder
            .Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("bigint")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(o => o.Type)
            .HasColumnName("type")
            .HasConversion<byte>()
            .HasColumnType("smallint")
            .HasDefaultValue(LogType.Add)
            .IsRequired();

        builder
            .Property(o => o.OrderId)
            .HasColumnName("order_id")
            .HasColumnType("bigint")
            .IsRequired();

        builder
            .Property(o => o.PatientId)
            .HasColumnName("patient_id")
            .HasColumnType("bigint")
            .IsRequired();

        builder
            .Property(o => o.Status)
            .HasColumnName("status")
            .HasConversion<byte>()
            .HasColumnType("smallint")
            .IsRequired();

        builder
            .Property(p => p.Remarks)
            .HasColumnName("remarks")
            .HasConversion(emptyStringToNullConverter)
            .HasMaxLength(255);

        builder
            .Property(p => p.LogBy)
            .HasColumnName("log_by")
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(p => p.LogAt)
            .HasColumnName("log_at")
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("TIMEZONE('utc', NOW())")
            .IsRequired();

        builder
            .HasIndex(p => new
            {
                p.OrderId,
                p.PatientId,
            })
            .HasDatabaseName("ix_patient_order_history_order_patient")
            .IncludeProperties(p => new
            {
                p.Status,
                p.LogAt,
            });

        builder
            .HasOne<PatientOrder>()
            .WithMany()
            .HasForeignKey(p => p.OrderId)
            .HasConstraintName("fk_patient_order_history_order_id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
