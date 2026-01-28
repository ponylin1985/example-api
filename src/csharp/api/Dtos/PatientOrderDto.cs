using Example.Api.Enums;

namespace Example.Api.Dtos;

/// <summary>
/// The data transfer object for a patient order.
/// </summary>
/// <value></value>
public record PatientOrderDto : BaseDto
{
    /// <summary>
    /// The unique identifier for the order.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// The instructions associated with the order.
    /// </summary>
    public string Instructions { get; init; } = string.Empty;

    /// <summary>
    /// The patient id related to the order.
    /// </summary>
    /// <value></value>
    public long PatientId { get; init; }

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
    /// Gets the list of prescriptions associated with the order.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<PrescriptionDto>? Prescriptions { get; set; }
}
