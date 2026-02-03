using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos.Requests;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Models;
using Example.Api.Repositories;
using Example.Api.Services.DomainServices;

namespace Example.Api.Processes;

/// <summary>
/// Process for adding a patient order.
/// </summary>
public sealed class AddPatientOrderProcess
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<AddPatientOrderProcess> _logger;

    /// <summary>
    /// The request dto for creating a patient order.
    /// </summary>
    private readonly CreatePatientOrderRequest _request;

    /// <summary>
    /// Patient information data repository.
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// Patient order data repository.
    /// </summary>
    private readonly IPatientOrderRepository _patientOrderRepository;

    /// <summary>
    /// Patient order history data repository.
    /// </summary>
    private readonly IPatientOrderHistoryRepository _patientOrderHistoryRepository;

    /// <summary>
    /// Patient order's prescription policy for validations.
    /// </summary>
    /// <value></value>
    private readonly IOrderPrescriptionPolicy _orderPrescriptionPolicy;

    /// <summary>
    /// DateTimeOffset provider for getting current time.
    /// </summary>
    /// <returns></returns>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// Valid patient statuses for order creation.
    /// </summary>
    private readonly PatientStatus[] _validStatuses = [PatientStatus.Active, PatientStatus.Transferred];

    /// <summary>
    /// Patient associated with the order.
    /// </summary>
    /// <value></value>
    public Patient? Patient { get; private set; }

    /// <summary>
    /// The patient order entity to be created.
    /// </summary>
    /// <value></value>
    public PatientOrder? Order { get; private set; }

    /// <summary>
    /// The created patient order after execution.
    /// </summary>
    /// <value></value>
    public PatientOrder? CreatedOrder { get; private set; }

    /// <summary>
    /// Constructor for AddOrderProcess.
    /// </summary>
    /// <param name="logger">The application logger.</param>
    /// <param name="request">The request dto for creating a patient order.</param>
    /// <param name="patientRepository">Patient information data repository.</param>
    /// <param name="patientOrderRepository">Patient order data repository.</param>
    /// <param name="patientOrderHistoryRepository">Patient order history data repository.</param>
    /// <param name="orderPrescriptionPolicy">The prescription policy for validating orders.</param>
    /// <param name="dateTimeOffsetProvider">The provider for current date and time.</param>
    public AddPatientOrderProcess(
        ILogger<AddPatientOrderProcess> logger,
        CreatePatientOrderRequest request,
        IPatientRepository patientRepository,
        IPatientOrderRepository patientOrderRepository,
        IPatientOrderHistoryRepository patientOrderHistoryRepository,
        IOrderPrescriptionPolicy orderPrescriptionPolicy,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _logger = logger;
        _request = request;
        _patientRepository = patientRepository;
        _patientOrderRepository = patientOrderRepository;
        _patientOrderHistoryRepository = patientOrderHistoryRepository;
        _orderPrescriptionPolicy = orderPrescriptionPolicy;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    /// <summary>
    /// Prepares the patient order entity from the request.
    /// </summary>
    /// <returns></returns>
    public AddPatientOrderProcess Prepare()
    {
        Order = MapToEntity(_request);
        return this;
    }

    /// <summary>
    /// Ensures the patient exists before creating the order.
    /// </summary>
    /// <returns></returns>
    public async Task<AddPatientOrderProcess> EnsurePatientExistAsync()
    {
        Patient = await _patientRepository.GetPatientAsync(Order!.PatientId);

        if (Patient is null)
        {
            _logger.LogWarning("Patient with ID {PatientId} not found.", Order.PatientId);
            throw new BusinessException(
                ApiCode.OperationFailed,
                $"Patient with ID {Order.PatientId} not found for order creation.");
        }

        return this;
    }

    /// <summary>
    /// Ensures the patient has a valid status before creating the order.
    /// </summary>
    /// <returns></returns>
    public AddPatientOrderProcess EnsurePatientStatus()
    {
        if (!_validStatuses.Contains(Patient!.Status))
        {
            _logger.LogWarning(
                "Patient with ID {PatientId} has invalid status {Status} for order creation.",
                Patient.Id,
                Patient.Status);

            throw new BusinessException(
                ApiCode.OperationFailed,
                "Patient status is invalid for creating orders.");
        }

        return this;
    }

    /// <summary>
    /// Ensures the medication IDs in the order prescriptions exist.
    /// </summary>
    /// <returns></returns>
    public async Task<AddPatientOrderProcess> EnsureMedicationIdExistAsync()
    {
        await _orderPrescriptionPolicy.EnsureMedicationIdsValidAsync(Order!);
        return this;
    }

    /// <summary>
    /// Executes the process to add the patient order.
    /// </summary>
    /// <returns></returns>
    public async Task<AddPatientOrderProcess> ExecuteAsync(IUnitOfWork unitOfWork)
    {
        CreatedOrder = await _patientOrderRepository.AddAsync(Order!);
        await unitOfWork.SaveChangesAsync();

        if (CreatedOrder is null or { Id: <= 0 })
        {
            _logger.LogError(
                "Failed to add order for PatientId: {PatientId}",
                Order!.PatientId);
            throw new BusinessException(
                ApiCode.OperationFailed,
                $"Failed to add order for PatientId: {Order!.PatientId}.");
        }

        var orderHistory = new PatientOrderHistory
        {
            Type = LogType.Add,
            OrderId = CreatedOrder.Id,
            PatientId = CreatedOrder!.PatientId,
            Status = CreatedOrder.Status,
            LogBy = CreatedOrder.CreatedBy,
            LogAt = CreatedOrder.CreatedAt,
            Remarks = CreatedOrder.Instructions,
        };

        await _patientOrderHistoryRepository.AddHistoryAsync(orderHistory);
        await unitOfWork.SaveChangesAsync();
        return this;
    }

    /// <summary>
    /// Ensures the order was created successfully.
    /// </summary>
    public void ShouldSuccessfully()
    {
        if (CreatedOrder is null or { Id: <= 0 })
        {
            _logger.LogError(
                "Failed to create order for PatientId: {PatientId}",
                Order!.PatientId);
            throw new BusinessException(
                ApiCode.OperationFailed,
                $"Failed to create order for PatientId: {Order!.PatientId}.");
        }
    }

    /// <summary>
    /// Maps CreateOrderRequest to PatientOrder entity.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private PatientOrder MapToEntity(CreatePatientOrderRequest request)
    {
        var userId = request.UserId!.Trim();
        var utcNow = _dateTimeOffsetProvider.UtcNow;

        var order = new PatientOrder
        {
            PatientId = request.PatientId!.Value,
            Instructions = request.Instructions!.Trim(),
            NextVisitDate = request.NextVisitDate?.UtcDateTime,
            StartDate = request.StartDate?.UtcDateTime,
            EndDate = request.EndDate?.UtcDateTime,
            Type = request.Type!.Value,
            Status = OrderStatus.Created,
            DispensedDate = request.DispensedDate?.UtcDateTime,
            CreatedBy = userId,
            CreatedAt = utcNow,
            UpdatedBy = userId,
            UpdatedAt = utcNow,
            Prescriptions = request.Prescriptions!.Select(p => new Prescription
            {
                MedicationId = p.MedicationId!.Value,
                Dose = p.Dose?.Trim(),
                Frequency = p.Frequency?.Trim(),
                DurationInDays = p.DurationInDays!.Value,
                Route = p.Route!.Value,
                CreatedBy = userId,
                CreatedAt = utcNow,
                UpdatedBy = userId,
                UpdatedAt = utcNow,
            }).ToList(),
        };

        return order;
    }
}
