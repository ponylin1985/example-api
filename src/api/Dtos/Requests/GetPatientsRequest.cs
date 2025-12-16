using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Example.Api.Dtos.Requests;

public record GetPatientsRequest : PagedRequest, IValidatableObject
{
    [Required]
    public required DateTimeOffset StartTime { get; init; }

    [Required]
    public required DateTimeOffset EndTime { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime < StartTime)
        {
            yield return new ValidationResult(
                "EndTime must be greater than or equal to StartTime.",
                [nameof(StartTime), nameof(EndTime)]);
        }
    }
}
