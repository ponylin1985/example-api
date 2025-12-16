using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to create a new order.
/// </summary>
public record CreateOrderRequest
{
    /// <summary>
    /// Gets the identifier of the patient who placed the order.
    /// </summary>
    [Required]
    public long PatientId { get; init; }

    /// <summary>
    /// Gets the message associated with the order.
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Message { get; init; } = string.Empty;
}
