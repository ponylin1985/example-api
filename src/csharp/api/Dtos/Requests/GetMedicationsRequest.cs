using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to retrieve medications with optional filtering and pagination.
/// </summary>
/// <value></value>
public record GetMedicationsRequest : PagedRequest
{
    /// <summary>
    /// Optional filter to retrieve only enabled medications.<para/>
    /// * If set to true, it will retrieve only enabled medications.<para/>
    /// * If set to false, it will retrieve only disabled medications.<para/>
    /// * If set to null, it will retrieve all medications regardless of their enabled status.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "isEnabled")]
    public bool? IsEnabled { get; init; }
}
