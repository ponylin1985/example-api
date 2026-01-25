namespace Example.Api.Dtos;

/// <summary>
/// The data transfer object for a medication.
/// </summary>
/// <value></value>
public record MedicationDto : BaseDto
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
}
