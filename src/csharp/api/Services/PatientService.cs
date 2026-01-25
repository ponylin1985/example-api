using Example.Api.DateTimeOffsetProviders;
using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
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
    /// <param name="repository">The patient repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PatientService(
        ILogger<PatientService> logger,
        IPatientRepository repository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request)
    {
        var queryResult = await _repository.GetPatientsAsync(
            request.StartTime,
            request.EndTime,
            request.PageNumber,
            request.PageSize);

        if (IsNoDataFound())
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

        bool IsNoDataFound() =>
            queryResult.Data.IsNullOrEmpty() || queryResult.TotalCount == 0;
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
        var patient = new Patient
        {
            Name = request.Name.Trim(),
            Orders =
            [
                new()
                {
                    Instructions = request.OrderMessage.Trim(),
                },
            ],
        };

        var createdPatient = await _repository.AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        if (!IsCreatedSuccessfully())
        {
            _logger.LogError("Failed to create patient: {Patient}", patient);
            return FailureResult<PatientDto>(ApiCode.OperationFailed, "Failed to create patient.");
        }

        return SuccessResult(createdPatient.ToDto());

        bool IsCreatedSuccessfully() =>
            createdPatient.Id != default;
    }
}
