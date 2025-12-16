using Example.Api.Infrastructure;

namespace Example.Api.Infrastructure;

public static class InfrastructureProvider
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDbSession, ApplicationDbSession>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
