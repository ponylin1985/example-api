using Example.Api.Dtos.Requests;
using Example.Api.Extensions;
using Example.Api.Services;

namespace Example.Api.Endpoints;

public static class PatientApiEndpoints
{
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
