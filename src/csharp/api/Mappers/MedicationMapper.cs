using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

/// <summary>
/// Mapper for converting Medication models to DTOs.
/// </summary>
public static class MedicationMapper
{
    /// <summary>
    /// Maps a Medication model to a MedicationDto.
    /// </summary>
    /// <param name="medication">Medication entity.</param>
    /// <returns>Medication DTO.</returns>
    public static MedicationDto ToDto(this Medication medication)
    {
        return new MedicationDto
        {
            Id = medication.Id,
            Name = medication.Name,
            Manufacturer = medication.Manufacturer,
            IsEnabled = medication.IsEnabled,
            CreatedAt = medication.CreatedAt,
            UpdatedAt = medication.UpdatedAt,
            CreatedBy = medication.CreatedBy,
            UpdatedBy = medication.UpdatedBy,
        };
    }

    /// <summary>
    /// Maps a collection of Medication models to a collection of MedicationDtos.
    /// </summary>
    /// <param name="medications">Collection of Medication entities.</param>
    /// <returns>Collection of Medication DTOs.</returns>
    public static IEnumerable<MedicationDto> ToDtos(this IEnumerable<Medication> medications)
    {
        return medications.Select(m => m.ToDto());
    }
}
