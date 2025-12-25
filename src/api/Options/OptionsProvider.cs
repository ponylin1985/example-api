namespace Example.Api.Options;

/// <summary>
/// Provides methods to add and configure options for the application.
/// </summary>
public static class OptionsProvider
{
    /// <summary>
    /// Adds and configures options for the application.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RequestResponseLoggingOptions>(configuration.GetSection("RequestResponseLoggingOptions"));
        return services;
    }
}
