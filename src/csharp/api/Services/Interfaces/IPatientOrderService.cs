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
    /// Retrieves an order by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> GetOrderAsync(long id);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">The creation request.</param>
    /// <returns>The created order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> AddPatientOrderAsync(CreatePatientOrderRequest request);

    /// <summary>
    /// Updates the instructions of an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="instructions">The new instructions.</param>
    /// <param name="userId">The ID of the user updating the order.</param>
    /// <returns>The updated order DTO.</returns>
    Task<ApiResult<PatientOrderDto>> UpdateInstructionsAsync(long id, string instructions, string userId);
}
