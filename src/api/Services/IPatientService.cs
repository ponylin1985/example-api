using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Models;

namespace Example.Api.Services;

/// <summary>
/// Service interface for managing patients.
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Retrieves patients within a specified date range.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResult<PagedResult<PatientDto>>> GetPatientsAsync(GetPatientsRequest request);

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<ApiResult<PatientDto>> CreatePatientAsync(CreatePatientRequest request);
}
