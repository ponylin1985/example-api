using System.Diagnostics;

namespace Example.Api.Middlewares;

/// <summary>
/// Middleware for adding TraceId to the logging scope.
/// </summary>
public class TraceIdMiddleware
{
    /// <summary>
    /// The next middleware in the pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger for the TraceIdMiddleware.
    /// </summary>
    private readonly ILogger<TraceIdMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TraceIdMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">Logger for the TraceIdMiddleware.</param>
    public TraceIdMiddleware(RequestDelegate next, ILogger<TraceIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
        using var _ = _logger.BeginScope(new Dictionary<string, object> { ["TraceId"] = traceId });
        await _next(context);
    }
}
