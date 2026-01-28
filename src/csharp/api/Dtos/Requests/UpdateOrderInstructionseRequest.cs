using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to update an order's instructions.
/// </summary>
public record UpdateOrderInstructionseRequest
{
    /// <summary>
    /// Instructions for the order.
    /// </summary>
    [Required]
    [DefaultValue("Some order instructions here...")]
    [MaxLength(500)]
    public string? Instructions { get; init; }

    /// <summary>
    /// The ID of the user updating the order.
    /// </summary>
    /// <value></value>
    [Required()]
    [MaxLength(50)]
    public string? UserId { get; init; }
}
