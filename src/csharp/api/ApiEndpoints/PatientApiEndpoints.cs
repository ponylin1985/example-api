using Example.Api.Dtos.Requests;
using Example.Api.Extensions;
using Example.Api.Services;

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

        group.MapGet("/", async (
            [AsParameters] GetPatientsRequest request,
            IPatientService patientService,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("PatientApiEndpoints");
            logger.LogInformation("Received request to get patients with parameters: {@Request}", request);

            var result = await patientService.GetPatientsAsync(request);

            if (!result.Success)
            {
                logger.LogWarning("Failed to retrieve patients: {Message}", result.Message);
            }
            else
            {
                logger.LogInformation("Successfully retrieved {Count} patients.", result.Data?.TotalCount ?? 0);
            }

            return result.ToHttpResult();
        })
        .WithName("GetPatientsByTimeRange")
        .WithDescription("Get patients created within a specified time range.");

        group.MapGet("/{id:long:min(1)}", async (
            long id,
            IPatientService patientService,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("PatientApiEndpoints");
            logger.LogInformation("Received request to get patient with ID: {PatientId}", id);

            var result = await patientService.GetPatientAsync(id);

            if (!result.Success)
            {
                logger.LogWarning("Failed to retrieve patient with ID {PatientId}: {Message}", id, result.Message);
            }
            else
            {
                logger.LogInformation("Successfully retrieved patient with ID: {PatientId}", id);
            }

            return result.ToHttpResult();
        })
        .WithName("GetPatientById")
        .WithDescription("Get a patient by their ID.");

        group.MapPost("/", async (
            CreatePatientRequest request,
            IPatientService patientService,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("PatientApiEndpoints");
            logger.LogInformation("Received request to create a new patient: {@Request}", request);

            var result = await patientService.CreatePatientAsync(request);

            if (!result.Success)
            {
                logger.LogWarning("Failed to create patient: {Message}", result.Message);
            }
            else
            {
                logger.LogInformation("Successfully created patient with ID: {PatientId}", result.Data?.Id);
            }

            return result.ToHttpResult();
        })
        .WithName("CreatePatient")
        .WithDescription("Create a new patient record.");

        return app;
    }
}
