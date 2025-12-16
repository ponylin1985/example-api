using System.ComponentModel.DataAnnotations;
using Ganss.Xss;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to create a new patient.
/// </summary>
/// <value></value>
public record CreatePatientRequest : IValidatableObject
{
    /// <summary>
    /// Name of the patient.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Message for the patient's order.
    /// </summary>
    /// <value></value>
    [Required]
    [MaxLength(500)]
    public string OrderMessage { get; init; } = string.Empty;

    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var sanitizer = validationContext.GetService<IHtmlSanitizer>();

        if (sanitizer is null)
        {
            throw new InvalidOperationException("IHtmlSanitizer service is not available for DTO validation.");
        }

        var errors = new List<string>();

        var sanitizedName = sanitizer.Sanitize(Name);

        if (Name != sanitizedName)
        {
            errors.Add("Name contains disallowed HTML or scripts.");
        }

        var sanitizedMessage = sanitizer.Sanitize(OrderMessage);

        if (OrderMessage != sanitizedMessage)
        {
            errors.Add("OrderMessage contains disallowed HTML or scripts.");
        }

        if (errors.Count > 0)
        {
            yield return new ValidationResult(
                string.Join(" ", errors),
                new[] { nameof(Name), nameof(OrderMessage) }
            );
        }
    }
}
