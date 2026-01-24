using Example.Api.Dtos.Requests;
using Example.Api.Dtos.Responses;
using Example.Api.Extensions;
using Example.Api.Services;
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
        MapCreatePatient(group);
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
            IPatientService patientService) =>
        {
            var result = await patientService.GetPatientsAsync(request);
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("GetPatientsByTimeRange")
        .WithDescription("Get patients created within a specified time range.")
        .CacheOutput(policy => policy
            .Expire(TimeSpan.FromMinutes(2))
            .SetVaryByQuery("startTime", "endTime", "pageNumber", "pageSize")
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
    /// Maps the CreatePatient endpoint.
    /// </summary>
    /// <param name="group"></param>
    private static void MapCreatePatient(RouteGroupBuilder group)
    {
        group.MapPost("/", async Task<RestApiResult> (
            CreatePatientRequest request,
            IPatientService patientService,
            IOutputCacheStore cacheStore) =>
        {
            var result = await patientService
                .CreatePatientAsync(request)
                .TapOnSuccessAsync(async () => await cacheStore.EvictByTagAsync("patients", default));
            return result.ToHttpResult();
        })
        .Produces<ApiResult>(StatusCodes.Status200OK)
        .Produces<ApiResult>(StatusCodes.Status400BadRequest)
        .Produces<ApiResult>(StatusCodes.Status500InternalServerError)
        .WithName("CreatePatient")
        .WithDescription("Create a new patient record.");
    }
}
