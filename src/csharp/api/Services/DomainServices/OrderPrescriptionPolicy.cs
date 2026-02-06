using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Infrastructure;

namespace Example.Api.Services.DomainServices;

/// <summary>
/// Implementation of order prescription policy validations.
/// </summary>
public class OrderPrescriptionPolicy : IOrderPrescriptionPolicy
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<OrderPrescriptionPolicy> _logger;

    /// <summary>
    /// Constructor for OrderPrescriptionPolicy.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    public OrderPrescriptionPolicy(
        ILogger<OrderPrescriptionPolicy> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public void EnsureMedicationIdsValid(
        IEnumerable<long> requestedMedicationIds,
        IEnumerable<long> existingMedicationIds)
    {
        if (requestedMedicationIds.IsNullOrEmpty())
        {
            _logger.LogWarning("No medication IDs provided in prescriptions.");
            throw new BusinessException(
                ApiCode.OperationFailed, "No medication IDs provided in prescriptions.");
        }

        if (existingMedicationIds.IsNullOrEmpty())
        {
            _logger.LogWarning("No existing medication IDs found for validation.");
            throw new BusinessException(
                ApiCode.OperationFailed, "One or more prescriptions have invalid medicationIds.");
        }

        requestedMedicationIds = requestedMedicationIds.Distinct().ToHashSet();
        existingMedicationIds = existingMedicationIds.Distinct().ToHashSet();

        var missingIds = requestedMedicationIds.Except(existingMedicationIds).ToList();

        if (!missingIds.IsNullOrEmpty())
        {
            _logger.LogWarning(
                "One or more medication IDs in prescriptions are invalid. Missing MedicationIds: {MissingIds}",
                string.Join(", ", missingIds));
            throw new BusinessException(
                ApiCode.OperationFailed, "One or more prescriptions have invalid medicationIds.");
        }
    }
}
