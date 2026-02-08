using Example.Api.Dtos;
using Example.Api.Dtos.Responses;

namespace Example.Api.Services;

/// <summary>
/// Service interface for managing medications.
/// </summary>
public interface IMedicationService
{
    /// <summary>
    /// Retrieves a paginated list of medications with optional filtering by enabled status.
    /// </summary>
    /// <param name="isEnabled">Filter for enabled medications.</param>
    /// <param name="pageNumber">The page number for pagination. Default is 1.</param>
    /// <param name="pageSize">The number of items per page. Default is 10.</param>
    /// <returns>A paginated list of medications.</returns>
    Task<ApiResult<PagedResult<MedicationDto>>> GetMedicationsAsync(
        bool? isEnabled = default,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// Retrieves a medication by its unique identifier.
    /// </summary>
    /// <param name="id">The id of the medication.</param>
    /// <returns>The medication if found, otherwise an appropriate result indicating not found.</returns>
    Task<ApiResult<MedicationDto>> GetMedicationAsync(long id);
}
