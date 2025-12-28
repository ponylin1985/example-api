namespace Example.Api.DateTimeOffsetProviders;

/// <summary>
/// Provider for retrieving the current date and time.
/// This abstraction allows for easier testing by mocking time.
/// </summary>
public interface IDateTimeOffsetProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
