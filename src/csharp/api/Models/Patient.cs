namespace Example.Api.Models;

/// <summary>
/// Represents a patient in the system.
/// </summary>
public record Patient : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier for the patient.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets the name of the patient.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of orders associated with the patient.
    /// </summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
