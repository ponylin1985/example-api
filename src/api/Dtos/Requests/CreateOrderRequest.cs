using Example.Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to create a new order.
/// </summary>
public record CreateOrderRequest : IValidatableObject
{
    /// <summary>
    /// The id of the patient related to the order.
    /// </summary>
    [Required]
    [Range(1, long.MaxValue, ErrorMessage = "PatientId must be greater than 0.")]
    public long PatientId { get; init; }

    /// <summary>
    /// Gets the message associated with the order.
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validator = validationContext.GetRequiredService<SanitizerValidator>();
        var isValid = validator.IsValid(Message, out var messageError);

        if (!isValid)
        {
            yield return new ValidationResult(
                messageError,
                new[] { nameof(Message) }
            );
        }
    }
}
