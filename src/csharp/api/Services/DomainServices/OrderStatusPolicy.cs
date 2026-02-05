using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Example.Api.Repositories;

namespace Example.Api.Services.DomainServices;

/// <summary>
/// Implementation of order status policy validations.
/// </summary>
public class OrderStatusPolicy : IOrderStatusPolicy
{
    /// <summary>
    /// The patient order repository.
    /// </summary>
    private readonly IPatientOrderRepository _patientOrderRepository;

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
    /// <param name="patientOrderRepository">The patient order repository.</param>
    /// <param name="logger">The application logger.</param>
    public OrderStatusPolicy(IPatientOrderRepository patientOrderRepository, ILogger<OrderStatusPolicy> logger)
    {
        _patientOrderRepository = patientOrderRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IOrderStatusPolicy> EnsurePatientOrderExistsAsync(long orderId)
    {
        _patientOrder = await _patientOrderRepository.GetPatientOrderAsync(orderId);

        if (_patientOrder is null || _patientOrder.Id <= 0)
        {
            _logger.LogWarning("Order with ID {Id} not found for patching.", orderId);
            throw new BusinessException(
                ApiCode.NoDataFound, $"Order with ID {orderId} not found.");
        }

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
