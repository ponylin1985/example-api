using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Example.Api.Repositories;

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
    /// Medication data repository.
    /// </summary>
    private readonly IMedicationRepository _medicationRepository;

    /// <summary>
    /// Constructor for OrderPrescriptionPolicy.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    /// <param name="medicationRepository">Medication data repository.</param>
    public OrderPrescriptionPolicy(
        ILogger<OrderPrescriptionPolicy> logger,
        IMedicationRepository medicationRepository)
    {
        _logger = logger;
        _medicationRepository = medicationRepository;
    }

    /// <inheritdoc/>
    public async Task EnsureMedicationIdsValidAsync(PatientOrder order)
    {
        IReadOnlyCollection<long> medicationIds = default!;
        int existingCount = default;

        ArgumentNullException.ThrowIfNull(order, nameof (order));
        ArgumentOutOfRangeException.ThrowIfLessThan(order.Prescriptions.Count, 1, nameof (order.Prescriptions));

        GivenMedicationIds();
        await WhenGetExistingMedicationCountAsync();
        ShouldAllMedicationIdsValid();

        void GivenMedicationIds()
        {
            medicationIds = order.Prescriptions
                .Select(p => p.MedicationId)
                .Distinct()
                .ToList();
        }

        async Task WhenGetExistingMedicationCountAsync()
        {
            existingCount = await _medicationRepository.GetExistingMedicationCountAsync(medicationIds);
        }

        void ShouldAllMedicationIdsValid()
        {
            if (existingCount != medicationIds.Count)
            {
                _logger.LogWarning(
                    "One or more medication IDs in prescriptions are invalid. Provided IDs: {MedicationIds}",
                    string.Join(", ", medicationIds));
                throw new BusinessException(
                    ApiCode.OperationFailed, "One or more prescriptions have invalid medication IDs.");
            }
        }   
    }}
