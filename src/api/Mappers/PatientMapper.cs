using Example.Api.Dtos;
using Example.Api.Models;

namespace Example.Api.Mappers;

public static class PatientMapper
{
    public static PatientDto ToDto(this Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name,
            CreatedAt = patient.CreatedAt,
            Orders = patient.Orders?.Select(o => new OrderDto
            {
                Id = o.Id,
                Message = o.Message,
                CreatedAt = o.CreatedAt,
            }).ToList() ?? Enumerable.Empty<OrderDto>().ToList(),
        };
    }

    public static IEnumerable<PatientDto> ToDtos(this IEnumerable<Patient> patients)
    {
        return patients.Select(p => p.ToDto());
    }
}
