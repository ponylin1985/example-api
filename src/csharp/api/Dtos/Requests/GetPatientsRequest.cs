using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

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
    [FromQuery(Name = "startTime")]
    public required DateTimeOffset StartTime { get; init; }

    /// <summary>
    /// The end time of the date range to filter patients. (UTC time)
    /// </summary>
    /// <value></value>
    [Required]
    [FromQuery(Name = "endTime")]
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

        var threeYearsLimit = TimeSpan.FromDays(3 * 365 + 1);
        var duration = EndTime - StartTime;

        if (duration > threeYearsLimit)
        {
            yield return new ValidationResult(
                $"The date range must not exceed 3 years. Requested duration was {duration.Days} days.",
                [nameof(StartTime), nameof(EndTime)]);
        }
    }
}
