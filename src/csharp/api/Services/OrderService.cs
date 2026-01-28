using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Mappers;
using Example.Api.Models;
using Example.Api.Repositories;
using Example.Api.Services.DomainServices;

namespace Example.Api.Services;

/// <summary>
/// Service for managing orders.
/// </summary>
public class OrderService : BaseService, IOrderService
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<OrderService> _logger;

    /// <summary>
    /// DateTimeOffset provider for getting current time.
    /// </summary>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// Order prescription policy for validations.
    /// </summary>
    private readonly IOrderPrescriptionPolicy _orderPrescriptionPolicy;

    /// <summary>
    /// Order data repository.
    /// </summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// Unit of work for managing transactions.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    /// <param name="dateTimeOffsetProvider">The date time offset provider.</param>
    /// <param name="orderPrescriptionPolicy">The order prescription policy.</param>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="patientRepository">The patient repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderService(
        ILogger<OrderService> logger,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        IOrderPrescriptionPolicy orderPrescriptionPolicy,
        IOrderRepository orderRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _orderPrescriptionPolicy = orderPrescriptionPolicy;
        _orderRepository = orderRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> GetOrderAsync(long id)
    {
        long orderId = default;
        PatientOrder? patientOrder = default;

        GivenOrderId();
        await WhenQueryingPatientAsync();
        ShouldFoundPatientOrder();
        return SuccessResult(patientOrder!.ToDto());

        void GivenOrderId() =>
            orderId = id;

        async Task WhenQueryingPatientAsync() =>
            patientOrder = await _orderRepository.GetOrderAsync(orderId);

        void ShouldFoundPatientOrder()
        {
            if (patientOrder is null)
            {
                _logger.LogWarning("Order with ID {Id} not found.", orderId);
                throw new BusinessException(ApiCode.NoDataFound, $"Order with ID {orderId} not found.");
            }
        }
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> AddPatientOrderAsync(CreateOrderRequest request)
    {
        PatientOrder? createdOrder = default;
        var order = MapToEntity(request);

        await EnsurePatientExists();
        await EnsurePrescriptionValidAsync();
        await WhenAddPatientOrder();
        ShouldCreatedSuccessfully();
        return SuccessResult(createdOrder!.ToDto());

        async Task EnsurePatientExists()
        {
            var patientExists = await _patientRepository.IsExistPatientAsync(order.PatientId);

            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for order creation.", order.PatientId);
                throw new BusinessException(
                    ApiCode.InvalidRequest,
                    $"Patient with ID {order.PatientId} does not exist.");
            }
        }

        async Task EnsurePrescriptionValidAsync()
        {
            await _orderPrescriptionPolicy.EnsureMedicationIdsValidAsync(order);
        }

        async Task WhenAddPatientOrder()
        {
            createdOrder = await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        void ShouldCreatedSuccessfully()
        {
            var errorMessage = $"Failed to create order for PatientId: {request.PatientId}.";

            if (createdOrder is null)
            {
                _logger.LogWarning(errorMessage);
                throw new BusinessException(
                    ApiCode.OperationFailed,
                    errorMessage);
            }

            if (createdOrder.Id == default)
            {
                _logger.LogWarning(errorMessage);
                throw new BusinessException(
                    ApiCode.OperationFailed,
                    errorMessage);
            }
        }
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> UpdateMessageAsync(long id, string message)
    {
        await using var _ = await _unitOfWork.BeginTransactionAsync();

        var utcNow = _dateTimeOffsetProvider.UtcNow;
        var updatedOrder = await _orderRepository.UpdateAsync(id, message.Trim(), utcNow);

        if (!IsUpdatedSuccessfully(out var order))
        {
            _logger.LogWarning("Order with ID {Id} not found for update.", id);
            return FailureResult<PatientOrderDto>(ApiCode.OperationFailed, $"Order with ID {id} not found.");
        }

        await _unitOfWork.CommitTransactionAsync();
        return SuccessResult(order.ToDto());

        bool IsUpdatedSuccessfully(out PatientOrder order)
        {
            order = default!;

            if (updatedOrder is null or { Id: 0, PatientId: 0 })
            {
                return false;
            }

            order = updatedOrder;
            return true;
        }
    }

    /// <summary>
    /// Maps CreateOrderRequest to PatientOrder entity.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private PatientOrder MapToEntity(CreateOrderRequest request)
    {
        var userId = request.UserId!.Trim();

        var order = new PatientOrder
        {
            PatientId = request.PatientId!.Value,
            Instructions = request.Instructions!.Trim(),
            NextVisitDate = request.NextVisitDate,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type!.Value,
            DispensedDate = request.DispensedDate,
            CreatedBy = userId,
            CreatedAt = _dateTimeOffsetProvider.UtcNow,
            UpdatedBy = userId,
            UpdatedAt = _dateTimeOffsetProvider.UtcNow,
            Prescriptions = request.Prescriptions!.Select(p => new Prescription
            {
                MedicationId = p.MedicationId!.Value,
                Dose = p.Dose!.Trim(),
                Frequency = p.Frequency!.Trim(),
                DurationInDays = p.DurationInDays!.Value,
                Route = p.Route!.Value,
                CreatedBy = userId,
                UpdatedBy = userId,
            }).ToList(),
        };

        return order;
    }
}
