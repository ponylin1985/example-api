using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;

namespace Example.Api.Services;

/// <summary>
/// Service interface for managing patients.
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Retrieves patients within a specified date range.
    /// </summary>
    /// <param name="request">The request containing date range and pagination information.</param>
    /// <returns>A paged result of patients.</returns>
    Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request);

    /// <summary>
    /// Retrieves a patient by their id.
    /// </summary>
    /// <param name="id">The ID of the patient to retrieve.</param>
    /// <returns>The patient with the specified ID.</returns>
    Task<ApiResult<PatientDto>> GetPatientAsync(long id);

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <param name="request">The request containing patient information.</param>
    /// <returns>The created patient.</returns>
    Task<ApiResult<PatientDto>> CreatePatientAsync(CreatePatientRequest request);
}
