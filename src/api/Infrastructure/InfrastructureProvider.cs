namespace Example.Api.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureProvider
{
    /// <summary>
    /// Registers infrastructure services with the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration to read settings from.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbSession, ApplicationDbSession>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("DefaultRedisConnection");
        });
        return services;
    }
}
