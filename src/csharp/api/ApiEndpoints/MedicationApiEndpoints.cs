using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Services;
using FluentValidation;
using RestApiResult = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.Ok<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.BadRequest<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.InternalServerError<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.StatusCodeHttpResult>;

namespace Example.Api.Endpoints;

/// <summary>
/// API endpoints extensions for medications.
/// </summary>
public static class MedicationApiEndpoints
{
    /// <summary>
    /// Maps medication-related API endpoints.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapMedicationApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/medications")
            .WithTags("Medications");
        MapGetMedications(group);
        MapGetMedication(group);
        return app;
    }

    /// <summary>
    /// Maps the GetMedications endpoint for retrieving a list of medications with optional filtering and pagination.
    /// </summary>
    /// <param name="group"></param>
    private static void MapGetMedications(RouteGroupBuilder group)
    {
        group.MapGet("/", async Task<RestApiResult> (
            [AsParameters] GetMedicationsRequest request,
            IMedicationService medicationService,
            IValidator<PagedRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResult<IDictionary<string, string[]>>
                {
                    Success = false,
                    Code = ApiCode.InvalidRequest,
                    Data = validationResult.ToDictionary(),
                }.ToHttpResult();
            }

            var result = await medicationService.GetMedicationsAsync(
                request.IsEnabled,
                request.PageNumber,
                request.PageSize);

            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("GetMedications")
        .WithDescription("Get a list of medications with optional filtering and pagination.")
        .CacheOutput(policy => policy
            .Expire(TimeSpan.FromMinutes(10))
            .SetVaryByQuery("*")
            .Tag("medications"));
    }

    /// <summary>
    /// Maps the GetMedication endpoint for retrieving a medication by its unique identifier.
    /// </summary>
    /// <param name="group"></param>
    public static void MapGetMedication(RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}", async Task<RestApiResult> (
            long id,
            IMedicationService medicationService) =>
        {
            var result = await medicationService.GetMedicationAsync(id);
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status404NotFound)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("GetMedicationById")
        .WithDescription("Get a medication by its unique identifier.")
        .CacheOutput(policy => policy
            .Expire(TimeSpan.FromMinutes(10))
            .Tag("medication"));
    }
}
