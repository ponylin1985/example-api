using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

/// <summary>
/// Mapper for Patient related objects.
/// </summary>
public static class PatientMapper
{
    /// <summary>
    /// Maps a Patient entity to a PatientDto.
    /// </summary>
    /// <param name="patient">The patient entity.</param>
    /// <returns>The patient DTO.</returns>
    public static PatientDto ToDto(this Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name,
            Age = patient.Age,
            Gender = patient.Gender,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            DateOfBirth = patient.DateOfBirth,
            Address = patient.Address,
            FirstVisitDate = patient.FirstVisitDate,
            Status = patient.Status,
            Remarks = patient.Remarks,
            CreatedAt = patient.CreatedAt,
            UpdatedAt = patient.UpdatedAt,
            Orders = patient.Orders?.ToDtos().ToList() ?? Enumerable.Empty<PatientOrderDto>().ToList(),
        };
    }

    /// <summary>
    /// Maps a collection of Patient entities to a collection of PatientDtos.
    /// </summary>
    /// <param name="patients">The collection of patient entities.</param>
    /// <returns>The collection of patient DTOs.</returns>
    public static IEnumerable<PatientDto> ToDtos(this IEnumerable<Patient> patients)
    {
        return patients.Select(p => p.ToDto());
    }
}
