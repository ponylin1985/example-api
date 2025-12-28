namespace Example.Api.DateTimeOffsetProviders;

/// <summary>
/// Extension methods for registering DateTimeOffsetProvider services.
/// </summary>
public static class DateTimeOffsetProviderExtensions
{
    /// <summary>
    /// Adds the default DateTimeOffsetProvider to the service collection.
    /// </summary>
    /// <param name="services"></param>
    public static void AddDateTimeOffsetProviders(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeOffsetProvider, DefaultDateTimeOffsetProvider>();
    }
}
