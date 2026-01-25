using Example.Api.Enums;

namespace Example.Api.Models;

/// <summary>
/// Represents a patient in the system.
/// </summary>
public record Patient : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the patient.
    /// </summary>
    public long Id { get; set; }

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
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the patient.
    /// </summary>
    /// <value></value>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the patient.
    /// </summary>
    /// <value></value>
    public Address Address { get; set; } =
        new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

    /// <summary>
    /// Gets or sets the full name of the patient.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Get or sets the date of birth of the patient.
    /// </summary>
    /// <value></value>
    public DateTimeOffset DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the first visit date of the patient.
    /// </summary>
    /// <value></value>
    public DateTimeOffset FirstVisitDate { get; set; }

    /// <summary>
    /// Get or sets the status of the patient.
    /// </summary>
    /// <value></value>
    public PatientStatus Status { get; set; }

    /// <summary>
    /// Get or sets any additional remarks about the patient.
    /// </summary>
    /// <value></value>
    public string? Remarks { get; set; }

    /// <summary>
    /// Gets or sets the collection of orders associated with the patient.
    /// </summary>
    public ICollection<PatientOrder> Orders { get; set; } = [];
}

/// <summary>
/// Pyhsical address representation.
/// </summary>
/// <param name="Country">Country</param>
/// <param name="City">City</param>
/// <param name="Area">Area</param>
/// <param name="Road">Road</param>
/// <param name="Street">Street</param>
/// <param name="Others">Others</param>
/// <returns></returns>
public record Address(string Country, string City, string Area, string Road, string Street, string Others);
