using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to get patients within a date range.
/// </summary>
/// <value></value>
public record GetPatientsRequest : PagedRequest
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
}
