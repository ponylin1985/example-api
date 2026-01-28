using Example.Api.Dtos.Requests;
using FluentValidation;
using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Provides extension methods to register validator implementations.
/// </summary>
public static class ValidatorsProvider
{
    /// <summary>
    /// Adds validator implementations to the service collection.
    /// </summary>
    /// <param name="services"></param>
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<PagedRequest>, PagedRequestValidator>();
        services.AddSingleton<IValidator<GetPatientsRequest>, GetPatientsRequestValidator>();
        services.AddSingleton<IValidator<CreatePatientRequest>, CreatePatientRequestValidator>();
        services.AddSingleton<IValidator<CreateOrderRequest>, CreateOrderRequestValidator>();
        services.AddSingleton<IValidator<UpdateOrderInstructionseRequest>, UpdateOrderInstructionseRequestValidator>();
        services.AddSingleton<IValidator<UpdatePatientRequest>, UpdatePatientRequestValidator>();
        services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
    }
}
