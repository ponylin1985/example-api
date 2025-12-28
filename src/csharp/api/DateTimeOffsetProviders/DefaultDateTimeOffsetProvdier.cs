namespace Example.Api.DateTimeOffsetProviders;

/// <summary>
/// Default implementation of IDateTimeOffsetProvider that returns the system's current UTC time.
/// </summary>
public class DefaultDateTimeOffsetProvider : IDateTimeOffsetProvider
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
