namespace Example.Api.Endpoints;

public static class ApiEndpointsProvider
{
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapPatientApiEndpoints();
        app.MapOrderApiEndpoints();
    }
}
