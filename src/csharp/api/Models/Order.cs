namespace Example.Api.Models;

/// <summary>
/// Represents an order placed by a patient.
/// </summary>
public record Order : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier for the order.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets the message associated with the order.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets the identifier of the patient who placed the order.
    /// </summary>
    public long PatientId { get; set; }

    /// <summary>
    /// Gets or sets the patient associated with the order.
    /// </summary>
    public Patient? Patient { get; set; }
}
