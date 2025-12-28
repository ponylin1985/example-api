namespace Example.Api.Models;

/// <summary>
/// Represents a patient in the system.
/// </summary>
public record Patient
{
    /// <summary>
    /// Gets the unique identifier for the patient.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the name of the patient.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The date and time when the patient record was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// The date and time when the patient record was last updated.
    /// </summary>
    /// <value></value>
    public DateTimeOffset UpdatedAt { get; init; }

    /// <summary>
    /// Gets or sets the collection of orders associated with the patient.
    /// </summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
