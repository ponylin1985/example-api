using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;

namespace Example.Api.Services.DomainServices;

/// <summary>
/// Implementation of order status policy validations.
/// </summary>
public class OrderStatusPolicy : IOrderStatusPolicy
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<OrderStatusPolicy> _logger;

    /// <summary>
    /// The patient order to validate.
    /// </summary>
    private PatientOrder? _patientOrder;

    /// <summary>
    /// Constructor for OrderStatusPolicy.
    /// </summary>
    /// <param name="logger">The application logger.</param>
    public OrderStatusPolicy(ILogger<OrderStatusPolicy> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public IOrderStatusPolicy EnsurePatientOrderExists(PatientOrder? patientOrder)
    {
        if (patientOrder is null || patientOrder.Id <= 0)
        {
            _logger.LogWarning("Order with ID {Id} not found for patching.", patientOrder?.Id);
            throw new BusinessException(
                ApiCode.NoDataFound, $"Order with ID {patientOrder?.Id} not found.");
        }

        _patientOrder = patientOrder;
        return this;
    }

    /// <inheritdoc/>
    public void EnsureCanBeDispensed()
    {
        if (!CanBeDispensed())
        {
            _logger.LogWarning(
                "Attempted to dispense order {OrderId} with invalid status {Status}.",
                _patientOrder?.Id,
                _patientOrder?.Status);
            throw new BusinessException(
                ApiCode.OperationFailed, "PatientOrder cannot be dispensed in its current status.");
        }
    }

    /// <inheritdoc/>
    public void EnsureCanBeExecuted()
    {
        if (!CanBeExecuted())
        {
            _logger.LogWarning(
                "Attempted to execute order {OrderId} with invalid status {Status}.",
                _patientOrder?.Id,
                _patientOrder?.Status);
            throw new BusinessException(
                ApiCode.OperationFailed, "PatientOrder cannot be executed in its current status.");
        }
    }

    /// <inheritdoc/>
    public void EnsureCanBeCancelled()
    {
        if (!CanBeCancelled())
        {
            _logger.LogWarning(
                "Attempted to cancel order {OrderId} with invalid status {Status}.",
                _patientOrder?.Id,
                _patientOrder?.Status);
            throw new BusinessException(
                ApiCode.OperationFailed, "PatientOrder cannot be cancelled in its current status.");
        }
    }

    /// <inheritdoc/>
    public bool CanBeDispensed()
    {
        var invalidStatues = new OrderStatus[]
        {
            OrderStatus.Cancelled,
            OrderStatus.Executed,
            OrderStatus.Expired,
        };

        if (invalidStatues.Contains(_patientOrder!.Status))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool CanBeExecuted()
    {
        var invalidStatues = new OrderStatus[]
        {
            OrderStatus.Cancelled,
            OrderStatus.Expired,
        };

        if (invalidStatues.Contains(_patientOrder!.Status))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public bool CanBeCancelled()
    {
        var invalidStatues = new OrderStatus[]
        {
            OrderStatus.Dispensed,
            OrderStatus.Executed,
            OrderStatus.Expired,
        };

        if (invalidStatues.Contains(_patientOrder!.Status))
        {
            return false;
        }

        return true;
    }
}
