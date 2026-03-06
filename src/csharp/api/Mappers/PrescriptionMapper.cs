using Dapper;
using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

/// <summary>
/// Mapper for converting Prescription models to DTOs.
/// </summary>
public static class PrescriptionMapper
{
    /// <summary>
    /// Maps a Prescription model to a PrescriptionDto.
    /// </summary>
    /// <param name="prescription"></param>
    /// <returns></returns>
    public static PrescriptionDto ToDto(this Prescription prescription)
    {
        return new PrescriptionDto
        {
            Id = prescription.Id,
            MedicationId = prescription.MedicationId,
            Dose = prescription.Dose,
            Frequency = prescription.Frequency,
            DurationInDays = prescription.DurationInDays,
            Route = prescription.Route,
            CreatedAt = prescription.CreatedAt,
            UpdatedAt = prescription.UpdatedAt,
            CreatedBy = prescription.CreatedBy,
            UpdatedBy = prescription.UpdatedBy,
        };
    }

    /// <summary>
    /// Maps a collection of Prescription models to a collection of PrescriptionDtos.
    /// </summary>
    /// <param name="prescriptions"></param>
    /// <returns></returns>
    public static IReadOnlyList<PrescriptionDto> ToDtos(this IEnumerable<Prescription> prescriptions)
    {
        return prescriptions.Select(p => p.ToDto()).AsList();
    }
}
