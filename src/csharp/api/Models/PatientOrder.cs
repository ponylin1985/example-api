using Example.Api.Enums;

namespace Example.Api.Models;

/// <summary>
/// Represents an order placed by a patient.
/// </summary>
public record PatientOrder : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier for the order.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets the instructions associated with the order.
    /// </summary>
    public string? Instructions { get; set; }

    /// <summary>
    /// The next visit date for the order.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? NextVisitDate { get; set; }

    /// <summary>
    /// The start date of the order.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// The end date of the order.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// The type of the order.
    /// </summary>
    /// <value></value>
    public OrderType Type { get; set; }

    /// <summary>
    /// The status of the order.
    /// </summary>
    /// <value></value>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// The date when the order was dispensed.
    /// </summary>
    /// <value></value>
    public DateTimeOffset? DispensedDate { get; set; }

    /// <summary>
    /// Gets the identifier of the patient who placed the order.
    /// </summary>
    public long PatientId { get; set; }

    /// <summary>
    /// Gets or sets the patient associated with the order.
    /// </summary>
    public Patient? Patient { get; set; }

    /// <summary>
    /// Gets the list of prescriptions associated with the order.
    /// </summary>
    /// <returns></returns>
    public ICollection<Prescription> Prescriptions { get; set; } = [];
}
