namespace Example.Api.Services;

public static class ServicesProvider
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}
