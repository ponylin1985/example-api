using Example.Api.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to create a new patient.
/// </summary>
/// <value></value>
public record CreatePatientRequest
{
    /// <summary>
    /// Name of the patient.
    /// </summary>
    [Required()]
    [MaxLength(50)]
    public string? Name { get; init; }

    /// <summary>
    /// Age of the patient.
    /// </summary>
    /// <value></value>
    [Required()]
    [Range(1, 150)]
    public int? Age { get; init; }

    /// <summary>
    /// Gender of the patient.
    /// </summary>
    /// <value></value>
    [Required()]
    [EnumDataType(typeof (Gender))]
    public Gender? Gender { get; init; }

    /// <summary>
    /// Email of the patient.
    /// </summary>
    /// <value></value>
    [MaxLength(100)]
    public string? Email { get; init; }

    /// <summary>
    /// Phone number of the patient.
    /// </summary>
    /// <value></value>
    [Required()]
    [MaxLength(10)]
    public string? PhoneNumber { get; init; }

    /// <summary>
    /// Birthday of the patient.
    /// </summary>
    /// <value></value>
    [Required()]
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; init; }

    /// <summary>
    /// The order associated with the patient.
    /// </summary>
    /// <value></value>
    [Required()]
    public CreatePatientOrderDto? Order { get; init; }
}

/// <summary>
/// DTO for creating a patient order.
/// </summary>
/// <value></value>
public record CreatePatientOrderDto
{
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
    [EnumDataType(typeof (OrderType))]
    public OrderType? Type { get; set; }

    /// <summary>
    /// The date when the order was dispensed.
    /// </summary>
    /// <value></value>
    [DataType(DataType.DateTime)]
    public DateTimeOffset? DispensedDate { get; set; }

    /// <summary>
    /// The prescriptions associated with the order.
    /// </summary>
    /// <value></value>
    [Required()]
    public ICollection<CreatePatientOrderPrescriptionDto>? Prescriptions { get; init; }
}

/// <summary>
/// DTO for creating a patient order prescription.
/// </summary>
/// <value></value>
public record CreatePatientOrderPrescriptionDto
{
    /// <summary>
    /// The MedicationId prescribed in the prescription.
    /// </summary>
    /// <value></value>
    [Required()]
    public long? MedicationId { get; set; }

    /// <summary>
    /// The dosage information for the prescription.
    /// </summary>
    /// <value></value>
    [Required()]
    [MaxLength(50)]
    public string? Dose { get; set; }

    /// <summary>
    /// The frequency of administration for the prescription.
    /// </summary>
    /// <value></value>
    [Required()]
    [MaxLength(50)]
    public string? Frequency { get; set; }

    /// <summary>
    /// The duration in days for the prescription.
    /// </summary>
    /// <value></value>
    [Required()]
    [Range(1, 365)]
    public int? DurationInDays { get; set; }

    /// <summary>
    /// The route of administration for the prescription.
    /// </summary>
    /// <value></value>
    [Required()]
    [MaxLength(50)]
    public MedicationRoute? Route { get; set; }
}
