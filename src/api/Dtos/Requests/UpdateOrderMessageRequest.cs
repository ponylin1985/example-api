using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to update an order's message.
/// </summary>
public record UpdateOrderMessageRequest
{
    /// <summary>
    /// Gets the new message for the order.
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Message { get; init; } = string.Empty;
}
