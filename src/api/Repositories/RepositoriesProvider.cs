namespace Example.Api.Repositories;

public static class RepositoriesProvider
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}
