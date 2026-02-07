namespace Example.Api.Models;

/// <summary>
/// Represents a medication in the system.
/// </summary>
/// <value></value>
public record Medication : BaseEntity
{
    /// <summary>
    /// The unique identifier for the medication.
    /// </summary>
    /// <value></value>
    public long Id { get; set; }

    /// <summary>
    /// The name of the medication.
    /// </summary>
    /// <value></value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The manufacturer of the medication.
    /// </summary>
    /// <value></value>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the medication is currently enabled for use in the system.
    /// </summary>
    /// <value></value>
    public bool IsEnabled { get; set; } = true;
}
