using Example.Api.Enums;

namespace Example.Api.Models;

/// <summary>
/// Represents the history of a patient order.
/// </summary>
public record PatientOrderHistory
{
    /// <summary>
    /// The unique identifier for the patient order history record.
    /// </summary>
    /// <value></value>
    public long Id { get; set; }

    /// <summary>
    /// The type of log entry.
    /// </summary>
    /// <value></value>
    public LogType Type { get; set; }

    /// <summary>
    /// The patient order identifier associated with this history record.
    /// </summary>
    /// <value></value>
    public long OrderId { get; set; }

    /// <summary>
    /// The identifier of the patient associated with this order history.
    /// </summary>
    /// <value></value>
    public long PatientId { get; set; }

    /// <summary>
    /// The status of the patient order at this point in history.
    /// </summary>
    /// <value></value>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// The remarks or notes associated with this history record.
    /// </summary>
    /// <value></value>
    public string? Remarks { get; set; }

    /// <summary>
    /// The user who logged this history record.
    /// </summary>
    /// <value></value>
    public string LogBy { get; set; } = default!;

    /// <summary>
    /// The timestamp when this history record was logged.
    /// </summary>
    /// <value></value>
    public DateTimeOffset LogAt { get; set; }
}
