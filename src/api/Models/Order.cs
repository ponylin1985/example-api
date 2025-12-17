namespace Example.Api.Models;

/// <summary>
/// Represents an order placed by a patient.
/// </summary>
public record Order
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
    /// Gets the identifier of the patient who placed the order.
    /// </summary>
    public long PatientId { get; init; }

    /// <summary>
    /// The date and time when the order was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// The date and time when the order was last updated.
    /// </summary>
    /// <value></value>
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>
    /// Gets or sets the patient associated with the order.
    /// </summary>
    public Patient? Patient { get; set; }
}
