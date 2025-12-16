using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Api.Dtos;

/// <summary>
/// The data transfer object for an patient order.
/// </summary>
/// <value></value>
public record OrderDto
{
    /// <summary>
    /// Gets the unique identifier for the order.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the message associated with the order.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The patient id related to the order.
    /// </summary>
    /// <value></value>
    public long PatientId { get; init; }

    /// <summary>
    /// Gets the date and time when the order was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
}
