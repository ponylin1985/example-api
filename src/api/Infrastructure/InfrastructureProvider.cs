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
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddInfrastructures(this IServiceCollection services)
    {
        services.AddScoped<IDbSession, ApplicationDbSession>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
