using Example.Api.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to get patients within a date range.
/// </summary>
/// <value></value>
public record GetPatientsRequest : PagedRequest
{
    /// <summary>
    /// The name to filter patients by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// Indicates whether the name filter is a prefix match. Default is false (exact match).
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "isPrefix")]
    [DefaultValue(false)]
    public bool IsPrefix { get; set; }

    /// <summary>
    /// The email to filter patients by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "email")]
    public string? Email { get; set; }

    /// <summary>
    /// The phone number to filter patients by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "phoneNumber")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// The status to filter patients by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "status")]
    [EnumDataType(typeof(PatientStatus))]
    public PatientStatus? Status { get; set; }

    /// <summary>
    /// The start time of the date range to filter patients. (UTC time)
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "startTime")]
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// The end time of the date range to filter patients. (UTC time)
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "endTime")]
    public DateTimeOffset? EndTime { get; set; }
}
