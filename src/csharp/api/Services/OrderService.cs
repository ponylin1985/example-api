using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using Example.Api.Mappers;
using Example.Api.Models;
using Example.Api.Repositories;

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
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="patientRepository">The patient repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderService(
        ILogger<OrderService> logger,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        IOrderRepository orderRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _orderRepository = orderRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResult<OrderDto>> GetOrderAsync(long id)
    {
        var order = await _orderRepository.GetOrderAsync(id);

        if (order is null)
        {
            _logger.LogWarning("Order with ID {Id} not found.", id);
            return NoDataFoundResult<OrderDto>();
        }

        return SuccessResult(order.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<OrderDto>> CreateOrderAsync(CreateOrderRequest request)
    {
        var patientExists = await _patientRepository.IsExistPatientAsync(request.PatientId);

        if (!patientExists)
        {
            _logger.LogWarning("Patient with ID {PatientId} not found for order creation.", request.PatientId);
            return FailureResult<OrderDto>(ApiCode.OperationFailed, $"Order with PatientId {request.PatientId} not found.");
        }

        var utcNow = _dateTimeOffsetProvider.UtcNow;

        var order = new Order
        {
            PatientId = request.PatientId,
            Message = request.Message,
        };

        var createdOrder = await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        if (createdOrder.Id == default)
        {
            _logger.LogWarning("Failed to create order for Patient ID {PatientId}.", request.PatientId);
            return FailureResult<OrderDto>(ApiCode.OperationFailed, "Failed to create the order.");
        }

        return SuccessResult(createdOrder.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<OrderDto>> UpdateMessageAsync(long id, string message)
    {
        await using var _ = await _unitOfWork.BeginTransactionAsync();

        var utcNow = _dateTimeOffsetProvider.UtcNow;
        var updatedOrder = await _orderRepository.UpdateAsync(id, message, utcNow);

        if (updatedOrder is null)
        {
            _logger.LogWarning("Order with ID {Id} not found for update.", id);
            return FailureResult<OrderDto>(ApiCode.OperationFailed, $"Order with ID {id} not found.");
        }

        await _unitOfWork.CommitTransactionAsync();
        return SuccessResult(updatedOrder.ToDto());
    }
}
