using Example.Api.Middlewares;

namespace Example.Api.Extensions;

/// <summary>
/// Extension methods for adding middleware.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds the TraceId middleware to the pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseTraceId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TraceIdMiddleware>();
    }

    /// <summary>
    /// Adds the global exception handler middleware to the pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }

    /// <summary>
    /// Adds the request and response logging middleware to the pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}
