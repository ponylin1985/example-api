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
    /// <param name="order">The order entity.</param>
    /// <returns>The order DTO.</returns>
    public static PatientOrderDto ToDto(this PatientOrder order)
    {
        return new PatientOrderDto
        {
            Id = order.Id,
            Instructions = order.Instructions,
            NextVisitDate = order.NextVisitDate,
            StartDate = order.StartDate,
            EndDate = order.EndDate,
            Type = order.Type,
            Status = order.Status,
            DispensedDate = order.DispensedDate,
            PatientId = order.PatientId,
            CreatedBy = order.CreatedBy,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            UpdatedBy = order.UpdatedBy,
        };
    }

    /// <summary>
    /// Maps a collection of PatientOrder entities to a collection of PatientOrderDtos.
    /// </summary>
    /// <param name="orders">The collection of order entities.</param>
    /// <returns>The collection of order DTOs.</returns>
    public static IEnumerable<PatientOrderDto> ToDtos(this IEnumerable<PatientOrder> orders)
    {
        return orders.Select(o => o.ToDto());
    }
}
