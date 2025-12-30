namespace Example.Api.Models;

/// <summary>
/// Base entity with common properties.
/// </summary>
/// <value></value>
public abstract record BaseEntity
{
    /// <summary>
    /// The date and time when the order was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the order was last updated.
    /// </summary>
    /// <value></value>
    public DateTimeOffset UpdatedAt { get; set; }
}
