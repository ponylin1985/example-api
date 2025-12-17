namespace Example.Api.Endpoints;

/// <summary>
/// Provides extension methods to map all API endpoints.
/// </summary>
public static class ApiEndpointsProvider
{
    /// <summary>
    /// Maps all API endpoints.
    /// </summary>
    /// <param name="app"></param>
    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapPatientApiEndpoints();
        app.MapOrderApiEndpoints();
    }
}
