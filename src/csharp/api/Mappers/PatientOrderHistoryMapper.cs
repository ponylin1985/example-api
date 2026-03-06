using Dapper;
using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

/// <summary>
/// Mapper extensions for PatientOrderHistory model and DTO.
/// </summary>
public static class PatientOrderHistoryMapper
{
    /// <summary>
    /// Maps PatientOrderHistory model to PatientOrderHistoryDto.
    /// </summary>
    /// <param name="model">The PatientOrderHistory model instance.</param>
    /// <returns>A PatientOrderHistoryDto instance.</returns>
    public static PatientOrderHistoryDto ToDto(this PatientOrderHistory model)
    {
        return new PatientOrderHistoryDto
        {
            Id = model.Id,
            Type = model.Type,
            OrderId = model.OrderId,
            PatientId = model.PatientId,
            Status = model.Status,
            Remarks = model.Remarks,
            LogBy = model.LogBy,
            LogAt = model.LogAt,
        };
    }

    /// <summary>
    /// Maps a collection of PatientOrderHistory models to PatientOrderHistoryDto collection.
    /// </summary>
    /// <param name="models">The collection of PatientOrderHistory model instances.</param>
    /// <returns>A collection of PatientOrderHistoryDto instances.</returns>
    public static IReadOnlyList<PatientOrderHistoryDto> ToDtos(this IEnumerable<PatientOrderHistory> models)
    {
        return models.Select(m => m.ToDto()).AsList();
    }
}
