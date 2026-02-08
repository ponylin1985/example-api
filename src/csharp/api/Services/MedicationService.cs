using Example.Api.Dtos;
using Example.Api.Dtos.Responses;
using Example.Api.Extensions;
using Example.Api.Mappers;
using Example.Api.Repositories;

namespace Example.Api.Services;

/// <summary>
/// Service implementation for managing medications.
/// </summary>
public class MedicationService : BaseService, IMedicationService
{
    /// <summary>
    /// Application logger.
    /// </summary>
    private readonly ILogger<MedicationService> _logger;

    /// <summary>
    /// Repository for accessing medication data.
    /// </summary>
    private readonly IMedicationRepository _medicationRepository;

    /// <summary>
    /// Constructor for MedicationService.
    /// </summary>
    /// <param name="logger">Application logger.</param>
    /// <param name="medicationRepository">Repository for accessing medication data.</param>
    public MedicationService(
        ILogger<MedicationService> logger,
        IMedicationRepository medicationRepository)
    {
        _logger = logger;
        _medicationRepository = medicationRepository;
    }

    /// <inheritdoc/>
    public async Task<ApiResult<PagedResult<MedicationDto>>> GetMedicationsAsync(
        bool? isEnabled = default,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var (Data, TotalCount) = await _medicationRepository.GetMedicationsAsync(isEnabled, pageNumber, pageSize);

        if (Data.IsNullOrEmpty() || TotalCount == 0)
        {
            _logger.LogInformation("No medications found with isEnabled={IsEnabled}.", isEnabled);
            return NoDataFoundPagedResult<MedicationDto>();
        }

        return SuccessPagedResult(Data.ToDtos(), pageNumber, pageSize, TotalCount);
    }

    /// <inheritdoc/>
    public async Task<ApiResult<MedicationDto>> GetMedicationAsync(long id)
    {
        var medication = await _medicationRepository.GetMedicationAsync(id);

        if (medication is null)
        {
            _logger.LogInformation("Medication with id={Id} not found.", id);
            return NoDataFoundResult<MedicationDto>();
        }

        return SuccessResult(medication.ToDto());
    }
}
