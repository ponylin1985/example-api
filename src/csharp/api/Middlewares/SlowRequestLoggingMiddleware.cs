using System.Diagnostics;

namespace Example.Api.Middlewares;

/// <summary>
/// Middleware for logging slow HTTP requests.
/// </summary>
public class SlowRequestLoggingMiddleware
{
    /// <summary>
    /// The next middleware in the pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger for the SlowRequestLoggingMiddleware.
    /// </summary>
    private readonly ILogger<SlowRequestLoggingMiddleware> _logger;

    /// <summary>
    /// Threshold in milliseconds to consider a request as slow.
    /// </summary>
    private readonly long _thresholdMs;

    /// <summary>
    /// Initializes a new instance of the <see cref="SlowRequestLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="thresholdMs">Threshold in milliseconds to consider a request as slow.</param>
    public SlowRequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<SlowRequestLoggingMiddleware> logger,
        long thresholdMs = 1000)
    {
        _next = next;
        _logger = logger;
        _thresholdMs = thresholdMs;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            
            if (sw.ElapsedMilliseconds > _thresholdMs)
            {
                _logger.LogWarning(
                    "Slow request detected:  {Method} {Path} took {Duration}ms (threshold: {Threshold}ms)",
                    context.Request.Method,
                    context.Request. Path,
                    sw.ElapsedMilliseconds,
                    _thresholdMs
                );
            }
        }
    }
}
