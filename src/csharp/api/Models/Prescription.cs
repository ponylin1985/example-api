using Example.Api.Enums;

namespace Example.Api.Models;

/// <summary>
/// Represents a prescription in the system.
/// </summary>
/// <value></value>
public record Prescription : BaseEntity
{
    /// <summary>
    /// The unique identifier for the prescription.
    /// </summary>
    /// <value></value>
    public long Id { get; set; }

    /// <summary>
    /// The OrderId associated with the prescription.
    /// </summary>
    /// <value></value>
    public long OrderId { get; set; }

    /// <summary>
    /// The MedicationId prescribed in the prescription.
    /// </summary>
    /// <value></value>
    public long MedicationId { get; set; }

    /// <summary>
    /// The dosage information for the prescription.
    /// </summary>
    /// <value></value>
    public string? Dose { get; set; }

    /// <summary>
    /// The frequency of administration for the prescription.
    /// </summary>
    /// <value></value>
    public string? Frequency { get; set; }

    /// <summary>
    /// The duration in days for the prescription.
    /// </summary>
    /// <value></value>
    public int DurationInDays { get; set; }

    /// <summary>
    /// The route of administration for the prescription.
    /// </summary>
    /// <value></value>
    public MedicationRoute Route { get; set; }

    /// <summary>
    /// The medication associated with the prescription.
    /// </summary>
    /// <value></value>
    public Medication? Medication { get; set; }
}
