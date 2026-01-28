using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to update an order's message.
/// </summary>
public record UpdateOrderMessageRequest
{
    /// <summary>
    /// Message to update the order with.
    /// </summary>
    [Required]
    [DefaultValue("Some order message here...")]
    [MaxLength(500)]
    public string Message { get; init; } = string.Empty;
}
