using Ganss.Xss;

namespace Example.Api.Validators;

/// <summary>
/// Validator for sanitizing input strings to prevent disallowed HTML or scripts.
/// </summary>
public class SanitizerValidator
{
    /// <summary>
    /// Validator for sanitizing input strings to prevent disallowed HTML or scripts.
    /// </summary>
    private readonly IHtmlSanitizer _sanitizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="SanitizerValidator"/> class.
    /// </summary>
    /// <param name="sanitizer"></param>
    public SanitizerValidator(IHtmlSanitizer sanitizer)
    {
        _sanitizer = sanitizer;
    }

    /// <summary>
    /// Validates the input string to ensure it does not contain disallowed HTML or scripts.
    /// </summary>
    /// <param name="input">The input string to validate.</param>
    /// <param name="errorMessage">An error message if validation fails.</param>
    /// <returns>True if the input is valid; otherwise, false.</returns>
    public bool IsValid(string input, out string errorMessage)
    {
        var sanitizedInput = _sanitizer.Sanitize(input);

        if (input != sanitizedInput)
        {
            errorMessage = "Input contains disallowed HTML or scripts.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
