using Example.Api.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to get patient orders with pagination.
/// </summary>
/// <value></value>
public record GetPatientOrdersRequest : PagedRequest
{
    /// <summary>
    /// The patient ID to filter orders by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "patientId")]
    public long? PatientId { get; init; }

    /// <summary>
    /// The type of orders to filter by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "type")]
    public OrderType? Type { get; init; }

    /// <summary>
    /// The status of orders to filter by.
    /// </summary>
    /// <value></value>
    [FromQuery(Name = "status")]
    public OrderStatus? Status { get; init; }
}
