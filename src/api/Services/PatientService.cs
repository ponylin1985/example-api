using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Mappers;
using Example.Api.Models;
using Example.Api.Repositories;

namespace Example.Api.Services;

public class PatientService : BaseService, IPatientService
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<PatientService> _logger;

    /// <summary>
    /// Patient data repository.
    /// </summary>
    private readonly IPatientRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="repository"></param>
    public PatientService(
        ILogger<PatientService> logger,
        IPatientRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

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

    /// <summary>
    /// Creates a new patient record.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ApiResult<PatientDto>> CreatePatientAsync(CreatePatientRequest request)
    {
        var utcNow = DateTimeOffset.UtcNow;

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

        // var existingPatient = await _repository.GetPatientByNameAsync(request.Name);

        // if (existingPatient is not null)
        // {
        //     _logger.LogInformation("Patient with name {Name} already exists with ID {Id}.", request.Name, existingPatient.Id);
        //     return BadRequestResult<PatientDto>(default!, $"Patient with name {request.Name} already exists.");
        // }

        var createdPatient = await _repository.CreatePatientAsync(patient);

        if (createdPatient.Id == default)
        {
            return FailureResult<PatientDto>(ApiCode.UnknownError, default!, "Failed to create patient.");
        }

        return SuccessResult(createdPatient.ToDto());
    }
}
