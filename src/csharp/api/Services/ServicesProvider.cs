using Example.Api.Services.DomainServices;

namespace Example.Api.Services;

/// <summary>
/// Provides extension methods to register service implementations.
/// </summary>
public static class ServicesProvider
{
    /// <summary>
    /// Adds service implementations to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IPatientOrderService, PatientOrderService>();
        services.AddScoped<IMedicationService, MedicationService>();
        services.AddSingleton<IOrderPrescriptionPolicy, OrderPrescriptionPolicy>();
        services.AddSingleton<IOrderStatusPolicy, OrderStatusPolicy>();
        return services;
    }
}
