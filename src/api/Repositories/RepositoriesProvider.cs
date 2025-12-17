namespace Example.Api.Repositories;

/// <summary>
/// Provides extension methods to register repository services.
/// </summary>
public static class RepositoriesProvider
{
    /// <summary>
    /// Adds repository services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add repositories to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}
