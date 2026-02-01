using Example.Api.Dtos;
using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;

namespace Example.Api.Services;

/// <summary>
/// Service interface for managing patient orders.
/// </summary>
public interface IPatientOrderService
{
    /// <summary>
    /// Retrieves a paginated list of patient orders based on the provided request parameters.
    /// </summary>
    /// <param name="request">The request containing pagination and filter parameters.</param>
    /// <returns>A paginated list of patient orders.</returns>
    Task<ApiResult<PagedResult<PatientOrderDto>>> GetPatientOrdersAsync(GetPatientOrdersRequest request);

    /// <summary>
    /// Retrieves an order by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> GetPatientOrderAsync(long id);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">The creation request.</param>
    /// <returns>The created patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> AddPatientOrderAsync(CreatePatientOrderRequest request);

    /// <summary>
    /// Updates the instructions of an existing patient order.
    /// </summary>
    /// <param name="id">The unique identifier of the patient order.</param>
    /// <param name="instructions">The new instructions.</param>
    /// <param name="userId">The ID of the user updating the order.</param>
    /// <returns>The updated patient order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> UpdateInstructionsAsync(long id, string instructions, string userId);
}
