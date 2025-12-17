using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
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
        try
        {
            var order = await _orderRepository.GetOrderAsync(id);
            return SuccessResult(order.ToDto());
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Order with ID {Id} not found.", id);
            return NoDataFoundResult<OrderDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order with ID {Id}.", id);
            return ErrorResult<OrderDto>("An error occurred while retrieving the order.");
        }
    }

    /// <inheritdoc />
    public async Task<ApiResult<OrderDto>> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            var patientExists = await _patientRepository.IsExistPatientAsync(request.PatientId);

            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for order creation.", request.PatientId);
                return NoDataFoundResult<OrderDto>($"Patient with PatientId {request.PatientId} not found.");
            }

            var utcNow = _dateTimeOffsetProvider.UtcNow;

            var order = new Order
            {
                PatientId = request.PatientId,
                Message = request.Message,
                CreatedAt = utcNow,
                UpdatedAt = utcNow,
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return SuccessResult(createdOrder.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for Patient ID {PatientId}.", request.PatientId);
            return ErrorResult<OrderDto>("An error occurred while creating the order.");
        }
    }

    /// <inheritdoc />
    public async Task<ApiResult<OrderDto>> UpdateOrderMessageAsync(long id, string message)
    {
        try
        {
            var order = new Order
            {
                Id = id,
                Message = message,
                UpdatedAt = _dateTimeOffsetProvider.UtcNow,
            };

            var updatedOrder = await _orderRepository.UpdateMessageAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return SuccessResult(updatedOrder.ToDto());
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            _logger.LogWarning("Order with ID {Id} not found for update.", id);
            return NoDataFoundResult<OrderDto>($"Order with ID {id} not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order with ID {Id}.", id);
            return ErrorResult<OrderDto>("An error occurred while updating the order.");
        }
    }
}
