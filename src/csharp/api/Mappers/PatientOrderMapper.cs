using Dapper;
using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

/// <summary>
/// Mapper for PatientOrder related objects.
/// </summary>
public static class PatientOrderMapper
{
    /// <summary>
    /// Maps a PatientOrder entity to a PatientOrderDto.
    /// </summary>
    /// <param name="patientOrder">The order entity.</param>
    /// <param name="includePrescriptions">Whether to include prescriptions in the DTO.</param>
    /// <returns>The order DTO.</returns>
    public static PatientOrderDto ToDto(this PatientOrder patientOrder, bool includePrescriptions = true)
    {
        var dto = new PatientOrderDto
        {
            Id = patientOrder.Id,
            Instructions = patientOrder.Instructions,
            NextVisitDate = patientOrder.NextVisitDate,
            StartDate = patientOrder.StartDate,
            EndDate = patientOrder.EndDate,
            Type = patientOrder.Type,
            Status = patientOrder.Status,
            DispensedDate = patientOrder.DispensedDate,
            PatientId = patientOrder.PatientId,
            CreatedBy = patientOrder.CreatedBy,
            CreatedAt = patientOrder.CreatedAt,
            UpdatedAt = patientOrder.UpdatedAt,
            UpdatedBy = patientOrder.UpdatedBy,
        };

        if (includePrescriptions)
        {
            dto.Prescriptions =
                patientOrder.Prescriptions?.ToDtos().ToList() ?? Enumerable.Empty<PrescriptionDto>().ToList();
        }

        return dto;
    }

    /// <summary>
    /// Maps a collection of PatientOrder entities to a collection of PatientOrderDtos.
    /// </summary>
    /// <param name="patientOrders">The collection of order entities.</param>
    /// <returns>The collection of order DTOs.</returns>
    public static IReadOnlyList<PatientOrderDto> ToDtos(this IEnumerable<PatientOrder> patientOrders)
    {
        return patientOrders.Select(o => o.ToDto()).AsList();
    }
}
