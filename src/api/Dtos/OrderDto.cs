namespace Example.Api.Dtos;

/// <summary>
/// The data transfer object for an patient order.
/// </summary>
/// <value></value>
public record OrderDto
{
    /// <summary>
    /// The unique identifier for the order.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// The message associated with the order.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The patient id related to the order.
    /// </summary>
    /// <value></value>
    public long PatientId { get; init; }

    /// <summary>
    /// The date and time when the order was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// The date and time when the order was last updated.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? UpdatedAt { get; init; }
}
