using Ganss.Xss;

namespace Example.Api.Validators;

public static class ValidatorsProvider
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddValidation();
        services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
        services.AddSingleton<SanitizerValidator>();
    }
}
