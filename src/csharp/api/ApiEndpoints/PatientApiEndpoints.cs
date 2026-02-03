using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Extensions;
using Example.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using RestApiResult = Microsoft.AspNetCore.Http.HttpResults.Results<
    Microsoft.AspNetCore.Http.HttpResults.Ok<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.BadRequest<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.InternalServerError<Example.Api.Dtos.Responses.ApiResult>,
    Microsoft.AspNetCore.Http.HttpResults.StatusCodeHttpResult>;

namespace Example.Api.Endpoints;

/// <summary>
/// API endpoints extensions for patients.
/// </summary>
public static class PatientApiEndpoints
{
    /// <summary>
    /// Maps patient-related API endpoints.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapPatientApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/patients")
            .WithTags("Patients");
        MapGetPatients(group);
        MapGetPatient(group);
        MapGetPatientOrderHistory(group);
        MapCreatePatient(group);
        MapUpdatePatient(group);
        return app;
    }

    /// <summary>
    /// Maps the GetPatients endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapGetPatients(RouteGroupBuilder group)
    {
        group.MapGet("/", async Task<RestApiResult> (
            [AsParameters] GetPatientsRequest request,
            IPatientService patientService,
            IValidator<GetPatientsRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResult<IDictionary<string, string[]>>
                {
                    Success = false,
                    Code = ApiCode.InvalidRequest,
                    Data = validationResult.ToDictionary(),
                    Message = "Invalid request data.",
                }.ToHttpResult();
            }

            var result = await patientService.GetPatientsAsync(request);
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("GetPatients")
        .WithDescription("Get patients with filters.")
        .CacheOutput(policy => policy
            .Expire(TimeSpan.FromMinutes(2))
            .SetVaryByQuery("*")
            .Tag("patients"));
    }

    /// <summary>
    /// Maps the GetPatient endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapGetPatient(RouteGroupBuilder group)
    {
        group.MapGet("/{id:long:min(1)}", async Task<RestApiResult> (
            long id,
            IPatientService patientService) =>
        {
            var result = await patientService.GetPatientAsync(id);
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("GetPatientById")
        .WithDescription("Get a patient by their ID.")
        .CacheOutput(policy => policy
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByRouteValue("id")
            .Tag("patient-detail"));
    }

    /// <summary>
    /// Maps the GetPatientOrderHistory endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapGetPatientOrderHistory(RouteGroupBuilder group)
    {
        group.MapGet("/{id:long:min(1)}/orders-histories", async Task<RestApiResult> (
            [FromRoute(Name = "id")] long patientId,
            [AsParameters] PagedRequest pagedRequest,
            IPatientOrderService patientOrderService) =>
        {
            var result = await patientOrderService.GetOrderHistoryByPatientIdAsync(
                patientId,
                pagedRequest.PageNumber,
                pagedRequest.PageSize);
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("GetPatientOrderHistoryByPatientId")
        .WithDescription("Get patient order history by patient identifier.");
    }

    /// <summary>
    /// Maps the CreatePatient endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapCreatePatient(RouteGroupBuilder group)
    {
        group.MapPost("/", async Task<RestApiResult> (
            CreatePatientRequest request,
            IPatientService patientService,
            IValidator<CreatePatientRequest> validator,
            IOutputCacheStore cacheStore) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResult<IDictionary<string, string[]>>
                {
                    Success = false,
                    Code = ApiCode.InvalidRequest,
                    Data = validationResult.ToDictionary(),
                    Message = "Invalid request data.",
                }.ToHttpResult();
            }

            var result = await patientService
                .AddPatientAsync(request)
                .TapOnSuccessAsync(async () => await cacheStore.EvictByTagAsync("patients", default));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("CreatePatient")
        .WithDescription("Create a new patient record.");
    }

    /// <summary>
    /// Maps the UpdatePatient endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapUpdatePatient(RouteGroupBuilder group)
    {
        group.MapPut("/{id:long:min(1)}", async Task<RestApiResult> (
            [FromRoute] long id,
            [FromBody] UpdatePatientRequest request,
            IPatientService patientService,
            IValidator<UpdatePatientRequest> validator,
            IOutputCacheStore cacheStore) =>
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return new ApiResult<IDictionary<string, string[]>>
                {
                    Success = false,
                    Code = ApiCode.InvalidRequest,
                    Data = validationResult.ToDictionary(),
                    Message = "Invalid request data.",
                }.ToHttpResult();
            }

            request = request with { Id = id };
            var result = await patientService
                .UpdatePatientAsync(request)
                .TapOnSuccessAsync(async () => await EvictPatientRelatedCaches(cacheStore));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("UpdatePatient")
        .WithDescription("Update an existing patient record.");
    }

    /// <summary>
    /// Evicts caches related to patients.
    /// </summary>
    /// <param name="cacheStore"></param>
    /// <returns></returns>
    private static async Task EvictPatientRelatedCaches(IOutputCacheStore cacheStore)
    {
        await cacheStore.EvictByTagAsync("patients", default);
        await cacheStore.EvictByTagAsync("patient-detail", default);
    }
}
