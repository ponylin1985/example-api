namespace Example.Api.Dtos;

/// <summary>
/// The base data transfer object.
/// </summary>
/// <value></value>
public abstract record BaseDto
{
    /// <summary>
    /// The user who created the data.
    /// </summary>
    /// <value></value>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the data was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The user who last updated the data.
    /// </summary>
    /// <value></value>
    public string UpdatedBy { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the data was last updated.
    /// </summary>
    /// <value></value>
    public DateTimeOffset UpdatedAt { get; set; }
}
