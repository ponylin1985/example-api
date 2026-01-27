using Example.Api.Enums;
using Example.Api.Models;

namespace Example.Api.Dtos;

/// <summary>
/// The data transfer object for a patient.
/// </summary>
/// <value></value>
public record PatientDto : BaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the patient.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the patient.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the age of the patient.
    /// </summary>
    /// <value></value>
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the gender of the patient.
    /// </summary>
    /// <value></value>
    public Gender Gender { get; set; }

    /// <summary>
    /// Get or sets the email of the patient.
    /// </summary>
    /// <value></value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the patient.
    /// </summary>
    /// <value></value>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    /// Gets or sets the address of the patient.
    /// </summary>
    /// <value></value>
    public Address? Address { get; set; }

    /// <summary>
    /// Get or sets the date of birth of the patient.
    /// </summary>
    /// <value></value>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the first visit date of the patient.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? FirstVisitDate { get; set; }

    /// <summary>
    /// Get or sets the status of the patient.
    /// </summary>
    /// <value></value>
    public PatientStatus Status { get; set; } = PatientStatus.Active;

    /// <summary>
    /// Get or sets any additional remarks about the patient.
    /// </summary>
    /// <value></value>
    public string? Remarks { get; set; }

    /// <summary>
    /// The patient's orders.
    /// </summary>
    /// <typeparam name="OrderDto"></typeparam>
    /// <returns></returns>
    public IReadOnlyList<PatientOrderDto> Orders { get; init; } = [];
}

