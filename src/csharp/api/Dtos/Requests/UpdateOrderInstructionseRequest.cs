using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Example.Api.Enums;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to patch an existing patient order.
/// </summary>
public record UpdatePatientOrderRequest
{
    /// <summary>
    /// The ID of the order to be updated.
    /// </summary>
    /// <value></value>
    internal long Id { get; init; }

    /// <summary>
    /// Status of the order.
    /// </summary>
    /// <value></value>
    internal OrderStatus Status { get; init; }

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
