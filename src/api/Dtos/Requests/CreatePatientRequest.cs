using Example.Api.Validators;
using System.ComponentModel.DataAnnotations;

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
        var validator = validationContext.GetRequiredService<SanitizerValidator>();
        var isValid = validator.IsValid(Name, out var nameError);

        if (!isValid)
        {
            yield return new ValidationResult(
                nameError,
                new[] { nameof(Name) }
            );
        }
    }
}
