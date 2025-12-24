using Example.Api.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to update an order's message.
/// </summary>
public record UpdateOrderMessageRequest : IValidatableObject
{
    /// <summary>
    /// Message to update the order with.
    /// </summary>
    [Required]
    [DefaultValue("Some order message here...")]
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
