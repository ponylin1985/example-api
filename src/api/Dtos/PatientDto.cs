namespace Example.Api.Dtos;

/// <summary>
/// The data transfer object for a patient.
/// </summary>
/// <value></value>
public record PatientDto
{
    /// <summary>
    /// The unique identifier for the patient.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Name of the patient.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Creation date of the patient record.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// The date and time when the patient record was last updated.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    /// The patient's orders.
    /// </summary>
    /// <typeparam name="OrderDto"></typeparam>
    /// <returns></returns>
    public ICollection<OrderDto> Orders { get; init; } = new List<OrderDto>();
}
