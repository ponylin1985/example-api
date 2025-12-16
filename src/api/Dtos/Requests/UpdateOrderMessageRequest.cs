using System.ComponentModel.DataAnnotations;
using Example.Api.Validators;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to update an order's message.
/// </summary>
public record UpdateOrderMessageRequest : IValidatableObject
{
    /// <summary>
    /// Gets the new message for the order.
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
