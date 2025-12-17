using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to get patients within a date range.
/// </summary>
/// <value></value>
public record GetPatientsRequest : PagedRequest, IValidatableObject
{
    /// <summary>
    /// The start time of the date range to filter patients. (UTC time)
    /// </summary>
    /// <value></value>
    [Required]
    public required DateTimeOffset StartTime { get; init; }

    /// <summary>
    /// The end time of the date range to filter patients. (UTC time)
    /// </summary>
    /// <value></value>
    [Required]
    public required DateTimeOffset EndTime { get; init; }

    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
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
