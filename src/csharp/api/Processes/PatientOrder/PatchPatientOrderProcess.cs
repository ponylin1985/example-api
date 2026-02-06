using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos.Requests;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Example.Api.Repositories;

namespace Example.Api.Processes;

/// <summary>
/// Process for patching (partially updating) a patient order.
/// </summary>
public sealed class PatchPatientOrderProcess
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<PatchPatientOrderProcess> _logger;

    /// <summary>
    /// The request dto for updating a patient order.
    /// </summary>
    private readonly UpdatePatientOrderRequest _request;

    /// <summary>
    /// Patient order data repository.
    /// </summary>
    private readonly IPatientOrderRepository _patientOrderRepository;

    /// <summary>
    /// Patient order history data repository.
    /// </summary>
    private readonly IPatientOrderHistoryRepository _patientOrderHistoryRepository;

    /// <summary>
    /// DateTimeOffset provider for getting current time.
    /// </summary>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// The patient order to be updated.
    /// </summary>
    /// <value></value>
    public PatientOrder? Order { get; private set; }

    /// <summary>
    /// The updated patient order.
    /// </summary>
    /// <value></value>
    public PatientOrder? UpdatedOrder { get; private set; }

    /// <summary>
    /// Constructor for PatchPatientOrderProcess.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="request"></param>
    /// <param name="patientOrderRepository"></param>
    /// <param name="patientOrderHistoryRepository"></param>
    /// <param name="dateTimeOffsetProvider"></param>
    public PatchPatientOrderProcess(
        ILogger<PatchPatientOrderProcess> logger,
        UpdatePatientOrderRequest request,
        IPatientOrderRepository patientOrderRepository,
        IPatientOrderHistoryRepository patientOrderHistoryRepository,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _request = request;
        _patientOrderRepository = patientOrderRepository;
        _patientOrderHistoryRepository = patientOrderHistoryRepository;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <summary>
    /// Prepares the patient order entity to be updated.
    /// </summary>
    /// <returns></returns>
    public PatchPatientOrderProcess Prepare()
    {
        Order = MapToEntity(_request);
        return this;
    }

    /// <summary>
    /// Executes the patch operation to update the patient order.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing transactions.</param>
    /// <param name="originalStatus">
    /// The original status of the order before update, 
    /// used for optimistic concurrency control.</param>
    /// <returns></returns>
    public async Task<PatchPatientOrderProcess> ExecuteAsync(
        IUnitOfWork unitOfWork,
        OrderStatus originalStatus)
    {
        UpdatedOrder = await _patientOrderRepository.PatchAsync(Order!, originalStatus);

        if (UpdatedOrder is null)
        {
            _logger.LogWarning("Patient order with ID {OrderId} not found for update.", Order!.Id);
            throw new BusinessException(ApiCode.OperationFailed,
                $"Patient order with ID {Order.Id} not found for update.");
        }

        var history = new PatientOrderHistory
        {
            Type = LogType.Update,
            OrderId = UpdatedOrder.Id,
            PatientId = UpdatedOrder.PatientId,
            Status = UpdatedOrder.Status,
            Remarks = UpdatedOrder.Instructions,
            LogBy = UpdatedOrder.UpdatedBy,
            LogAt = UpdatedOrder.UpdatedAt,
        };

        var orderHistory = await _patientOrderHistoryRepository.AddHistoryAsync(history);
        await unitOfWork.SaveChangesAsync();

        if (orderHistory is null or { Id: <= 0 })
        {
            _logger.LogWarning("Failed to log history for updated patient order with ID {OrderId}.",
                UpdatedOrder.Id);
            throw new BusinessException(ApiCode.OperationFailed,
                $"Failed to log history for updated patient order with ID {UpdatedOrder.Id}.");
        }

        return this;
    }

    /// <summary>
    /// Validates that the update was successful.
    /// </summary>
    public void ShouldSuccessfully()
    {
        if (UpdatedOrder is null or { Id: <= 0 })
        {
            _logger.LogWarning("Failed to update patient order with ID {OrderId}.", Order!.Id);
            throw new BusinessException(ApiCode.OperationFailed,
                $"Failed to update patient order with ID {Order.Id}.");
        }
    }

    /// <summary>
    /// Maps the update request to a PatientOrder entity.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private PatientOrder MapToEntity(UpdatePatientOrderRequest request)
    {
        var userId = request.UserId!.Trim();
        var utcNow = _dateTimeOffsetProvider.UtcNow;

        var order = new PatientOrder
        {
            Id = request.Id,
            Instructions = request.Instructions!,
            Status = request.Status,
            UpdatedBy = userId,
            UpdatedAt = utcNow,
        };

        return order;
    }
}
