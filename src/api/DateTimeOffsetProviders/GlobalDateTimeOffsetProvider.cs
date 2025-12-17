namespace Example.Api.DateTimeOffsetProviders;

/// <summary>
/// Global static access to the current IDateTimeOffsetProvider.
/// </summary>
public static class GlobalDateTimeOffsetProvider
{
    /// <summary>
    /// The current IDateTimeOffsetProvider instance.
    /// </summary>
    /// <returns></returns>
    private static IDateTimeOffsetProvider _current = new DefaultDateTimeOffsetProvider();

    /// <summary>
    /// Lock object for thread safety.
    /// </summary>
    /// <returns></returns>
    private static readonly Lock _lock = new();

    /// <summary>
    /// Gets the current UTC date and time from the current provider.
    /// </summary>
    public static DateTimeOffset UtcNow
    {
        get
        {
            lock (_lock)
            {
                return _current.UtcNow;
            }
        }
    }

    /// <summary>
    /// Resets the current provider to the default implementation. Usually used in testing.
    /// </summary>
    public static void Reset(IDateTimeOffsetProvider? provider = default)
    {
        lock (_lock)
        {
            _current = provider ?? new DefaultDateTimeOffsetProvider();
        }
    }
}
