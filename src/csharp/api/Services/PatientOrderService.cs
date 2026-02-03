using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Infrastructure;
using Example.Api.Mappers;
using Example.Api.Processes;
using Example.Api.Repositories;
using Example.Api.Services.DomainServices;

namespace Example.Api.Services;

/// <summary>
/// Service for managing patient orders.
/// </summary>
public class PatientOrderService : BaseService, IPatientOrderService
{
    /// <summary>
    /// Application logger factory.
    /// </summary>
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<PatientOrderService> _logger;

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
    private readonly IPatientOrderRepository _patientOrderRepository;

    /// <summary>
    /// Order history data repository.
    /// </summary>
    private readonly IPatientOrderHistoryRepository _patientOrderHistoryRepository;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _patientRepository;

    /// <summary>
    /// Unit of work for managing transactions.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientOrderService"/> class.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    /// <param name="dateTimeOffsetProvider">The date time offset provider.</param>
    /// <param name="orderPrescriptionPolicy">The order prescription policy.</param>
    /// <param name="patientOrderRepository">The order repository.</param>
    /// <param name="patientOrderHistoryRepository">The patient order history repository.</param>
    /// <param name="patientRepository">The patient repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PatientOrderService(
        ILoggerFactory loggerFactory,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        IOrderPrescriptionPolicy orderPrescriptionPolicy,
        IPatientOrderRepository patientOrderRepository,
        IPatientOrderHistoryRepository patientOrderHistoryRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<PatientOrderService>();
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _orderPrescriptionPolicy = orderPrescriptionPolicy;
        _patientOrderRepository = patientOrderRepository;
        _patientOrderHistoryRepository = patientOrderHistoryRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientOrderDto>>> GetPatientOrdersAsync(
        GetPatientOrdersRequest request)
    {
        var queryResult = await _patientOrderRepository.GetPatientOrdersAsync(
            request.PageNumber,
            request.PageSize,
            request.PatientId,
            request.Type,
            request.Status);

        if (!HasPatientOrdersData())
        {
            _logger.LogInformation(
                "No patient orders found for PatientId: {PatientId}, Type: {Type}, Status: {Status}",
                request.PatientId,
                request.Type,
                request.Status);
            return NoDataFoundPagedResult<PatientOrderDto>();
        }

        var dtos = queryResult.Data!.ToDtos();
        return SuccessPagedResult(
            dtos,
            request.PageNumber,
            request.PageSize,
            queryResult.TotalCount);

        bool HasPatientOrdersData() =>
            !queryResult.Data.IsNullOrEmpty() && queryResult.TotalCount > 0;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> GetPatientOrderAsync(long id)
    {
        var patientOrder = await _patientOrderRepository.GetPatientOrderAsync(id);

        if (patientOrder is null or { Id: <= 0 })
        {
            _logger.LogWarning("Order with ID {Id} not found.", id);
            return NoDataFoundResult<PatientOrderDto>($"Order with ID {id} not found.");
        }

        return SuccessResult(patientOrder.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientOrderHistoryDto>>> GetOrderHistoryByPatientIdAsync(
        long patientId,
        int pageNumber,
        int pageSize)
    {
        var queryResult = await _patientOrderHistoryRepository.GetHistoriesByPatientIdAsync(
            patientId,
            pageNumber,
            pageSize);

        if (!HasOrderHistoriesData())
        {
            _logger.LogInformation(
                "No patient order histories found for PatientId: {PatientId}",
                patientId);
            return NoDataFoundPagedResult<PatientOrderHistoryDto>();
        }

        var dtos = queryResult.Data!.ToDtos();
        return SuccessPagedResult(
            dtos,
            pageNumber,
            pageSize,
            queryResult.TotalCount);

        bool HasOrderHistoriesData() =>
            !queryResult.Data.IsNullOrEmpty() && queryResult.TotalCount > 0;
    }

    public async Task<ApiResult<PagedResult<PatientOrderHistoryDto>>> GetOrderHistoryByOrderIdAsync(
        long orderId,
        int pageNumber,
        int pageSize)
    {
        var queryResult = await _patientOrderHistoryRepository.GetHistoriesByOrderIdAsync(
            orderId,
            pageNumber,
            pageSize);

        if (!HasOrderHistoriesData())
        {
            _logger.LogInformation(
                "No patient order histories found for OrderId: {OrderId}",
                orderId);
            return NoDataFoundPagedResult<PatientOrderHistoryDto>();
        }

        var dtos = queryResult.Data!.ToDtos();
        return SuccessPagedResult(
            dtos,
            pageNumber,
            pageSize,
            queryResult.TotalCount);

        bool HasOrderHistoriesData() =>
            !queryResult.Data.IsNullOrEmpty() && queryResult.TotalCount > 0;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> AddPatientOrderAsync(CreatePatientOrderRequest request)
    {
        var process = new AddPatientOrderProcess(
            _loggerFactory.CreateLogger<AddPatientOrderProcess>(),
            request,
            _patientRepository,
            _patientOrderRepository,
            _patientOrderHistoryRepository,
            _orderPrescriptionPolicy,
            _dateTimeOffsetProvider);

        await _unitOfWork.ExecuteStrategyAsync(async () =>
        {
            try
            {
                await using var _ = await _unitOfWork.BeginTransactionAsync();
                await process
                    .Prepare()
                    .EnsurePatientExistAsync()
                    .Then(p => p.EnsurePatientStatus())
                    .ThenAsync(p => p.EnsureMedicationIdExistAsync())
                    .ThenAsync(p => p.ExecuteAsync(_unitOfWork))
                    .Then(p => p.ShouldSuccessfully());
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An error occurred while adding a new patient order. Request: {@Request}", request);
                await _unitOfWork.RollbackTransactionAsync();
                throw new BusinessException(ApiCode.OperationFailed, "Failed to add new patient order.");
            }
        });

        return SuccessResult(process.CreatedOrder!.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> DispenseOrderAsync(UpdatePatientOrderRequest request)
    {
        return await PatchPatientOrderAsync(
            request with { Status = OrderStatus.Dispensed });
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> ExecuteOrderAsync(UpdatePatientOrderRequest request)
    {
        return await PatchPatientOrderAsync(
            request with { Status = OrderStatus.Executed });
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientOrderDto>> CancelOrderAsync(UpdatePatientOrderRequest request)
    {
        return await PatchPatientOrderAsync(
            request with { Status = OrderStatus.Cancelled });
    }

    /// <summary>
    /// Patches an existing patient order.
    /// </summary>
    /// <param name="request">Patch patient order request.</param>
    /// <returns>The updated patient order DTO.</returns>
    private async Task<ApiResult<PatientOrderDto>> PatchPatientOrderAsync(UpdatePatientOrderRequest request)
    {
        var process = new PatchPatientOrderProcess(
            _loggerFactory.CreateLogger<PatchPatientOrderProcess>(),
            request,
            _patientOrderRepository,
            _patientOrderHistoryRepository,
            _dateTimeOffsetProvider);

        await _unitOfWork.ExecuteStrategyAsync(async () =>
        {
            try
            {
                await using var _ = await _unitOfWork.BeginTransactionAsync();
                await process
                    .Prepare()
                    .ExecuteAsync(_unitOfWork)
                    .Then(p => p.ShouldSuccessfully());
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An error occurred while patching patient order with ID {OrderId}. Request: {@Request}",
                    process.Order!.Id,
                    request);
                await _unitOfWork.RollbackTransactionAsync();
                throw new BusinessException(ApiCode.OperationFailed, "Failed to update patient order.");
            }
        });

        return SuccessResult(process.UpdatedOrder!.ToDto(includePrescriptions: false));
    }
}
