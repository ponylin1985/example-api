using Example.Api.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to create a new order related to a patient.
/// </summary>
public record CreatePatientOrderRequest
{
    /// <summary>
    /// The id of the patient related to the order.
    /// </summary>
    [Required]
    [DefaultValue(1L)]
    [Range(1, long.MaxValue)]
    public long? PatientId { get; init; }

    /// <summary>
    /// Instructions for the patient's order.
    /// </summary>
    [Required()]
    [DefaultValue("Some instructions for the patient's order.")]
    [MaxLength(500)]
    public string? Instructions { get; init; }

    /// <summary>
    /// The next visit date for the order.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? NextVisitDate { get; set; }

    /// <summary>
    /// The start date of the order.
    /// </summary>
    /// <value></value>
    [DataType(DataType.DateTime)]
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// The end date of the order.
    /// </summary>
    /// <value></value>
    [DataType(DataType.DateTime)]
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// The type of the order.
    /// </summary>
    /// <value></value>
    [Required()]
    [EnumDataType(typeof(OrderType))]
    public OrderType? Type { get; set; }

    /// <summary>
    /// The date when the order was dispensed.
    /// </summary>
    /// <value></value>
    [DataType(DataType.DateTime)]
    public DateTimeOffset? DispensedDate { get; set; }

    /// <summary>
    /// The user ID associated with the order.
    /// </summary>
    /// <value></value>
    public string? UserId { get; init; }

    /// <summary>
    /// The prescriptions associated with the order.
    /// </summary>
    /// <value></value>
    [Required()]
    public ICollection<CreatePatientOrderPrescriptionDto>? Prescriptions { get; init; }
}
