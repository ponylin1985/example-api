using FluentValidation;
using FluentValidation.Validators;
using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Property validator that uses SanitizerValidator to validate string properties.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SanitizerPropertyValidator<T> : PropertyValidator<T, string>
{
    /// <summary>
    /// The HTML sanitizer.
    /// </summary>
    private readonly IHtmlSanitizer _sanitizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SanitizerPropertyValidator{T}"/> class.
    /// </summary>
    /// <param name="sanitizer"></param>
    public SanitizerPropertyValidator(IHtmlSanitizer sanitizer)
    {
        _sanitizer = sanitizer;
    }

    /// <summary>
    /// Validates the specified value.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ValidationContext<T> context, string value)
    {
        return value == _sanitizer.Sanitize(value);
    }

    /// <summary>
    /// Gets the name of the validator.
    /// </summary>
    public override string Name => "SanitizerValidator";

    /// <summary>
    /// Gets the default message template.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "{PropertyName} contains disallowed HTML or scripts.";
}

/// <summary>
/// Provides extension methods for SanitizerValidator.
/// </summary>
public static class SanitizerValidatorExtensions
{
    /// <summary>
    /// Adds a sanitizer validator to the rule builder.
    /// </summary>
    /// <param name="ruleBuilder"></param>
    /// <param name="sanitizer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> Sanitized<T>(
        this IRuleBuilder<T, string> ruleBuilder, IHtmlSanitizer sanitizer)
    {
        return ruleBuilder.SetValidator(new SanitizerPropertyValidator<T>(sanitizer));
    }
}
