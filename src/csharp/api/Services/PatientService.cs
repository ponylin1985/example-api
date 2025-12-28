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
/// Service for managing patients.
/// </summary>
public class PatientService : BaseService, IPatientService
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<PatientService> _logger;

    /// <summary>
    /// DateTimeOffset provider for getting current time.
    /// </summary>
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Unit of work for managing transactions.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientService"/> class.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    /// <param name="dateTimeOffsetProvider">The date time offset provider.</param>
    /// <param name="repository">The patient repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PatientService(
        ILogger<PatientService> logger,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        IPatientRepository repository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request)
    {
        var threeYearsLimit = TimeSpan.FromDays(3 * 365 + 1);
        var duration = request.EndTime - request.StartTime;

        if (duration > threeYearsLimit)
        {
            _logger.LogWarning("Query range exceeded 3 years limit. Requested duration: {Duration}", duration);

            return BadRequestResult<PagedResult<PatientDto>>(
                default!,
                $"The date range must not exceed 3 years. Requested duration was {duration.Days} days.");
        }

        var queryResult = await _repository.GetPatientsAsync(
            request.StartTime,
            request.EndTime,
            request.PageNumber,
            request.PageSize);

        if (queryResult.TotalCount == 0)
        {
            _logger.LogInformation(
                "No patients found for the given date range: {StartTime} to {EndTime}",
                request.StartTime,
                request.EndTime);
            return NoDataFoundPagedResult<PatientDto>();
        }

        var dtos = queryResult.Data.ToDtos();
        return SuccessPagedResult(
            dtos,
            request.PageNumber,
            request.PageSize,
            queryResult.TotalCount);
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> GetPatientAsync(long id)
    {
        var patient = await _repository.GetPatientAsync(id);

        if (patient is null)
        {
            _logger.LogInformation("Patient with ID {Id} not found.", id);
            return NoDataFoundResult<PatientDto>();
        }

        return SuccessResult(patient.ToDto());
    }

    /// <inheritdoc />
    public async Task<ApiResult<PatientDto>> CreatePatientAsync(CreatePatientRequest request)
    {
        var utcNow = _dateTimeOffsetProvider.UtcNow;

        var patient = new Patient
        {
            Name = request.Name,
            CreatedAt = utcNow,
            Orders =
            [
                new Order
                {
                    Message = request.OrderMessage,
                    CreatedAt = utcNow,
                    UpdatedAt = utcNow,
                }
            ],
        };

        var createdPatient = await _repository.CreatePatientAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        if (createdPatient.Id == default)
        {
            return FailureResult<PatientDto>(ApiCode.OperationFailed, "Failed to create patient.");
        }

        return SuccessResult(createdPatient.ToDto());
    }
}
