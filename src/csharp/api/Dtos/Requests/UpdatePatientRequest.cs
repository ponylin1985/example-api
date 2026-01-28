using Example.Api.Enums;
using Example.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Dtos.Requests;

/// <summary>
/// Request to update an existing patient.
/// </summary>
/// <value></value>
public record UpdatePatientRequest
{
    /// <summary>
    /// The patient id to update.
    /// </summary>
    /// <value></value>
    internal long Id { get; set; }

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
    /// Address of the patient.
    /// </summary>
    /// <value></value>
    public Address? Address { get; init; }

    /// <summary>
    /// Additional remarks about the patient.
    /// </summary>
    /// <value></value>
    [MaxLength(500)]
    public string? Remarks { get; init; }

    /// <summary>
    /// The ID of the user creating the patient.
    /// </summary>
    /// <value></value>
    [Required()]
    [MaxLength(50)]
    public string? UserId { get; init; }
}
