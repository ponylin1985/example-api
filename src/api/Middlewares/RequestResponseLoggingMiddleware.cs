using Example.Api.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace Example.Api.Middlewares;

/// <summary>
/// Middleware for logging HTTP requests and responses.
/// </summary>
public class RequestResponseLoggingMiddleware
{
    /// <summary>
    /// The next middleware in the pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger for the RequestResponseLoggingMiddleware.
    /// </summary>
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    /// <summary>
    /// Options monitor for request and response logging.
    /// </summary>
    private readonly IOptionsMonitor<RequestResponseLoggingOptions> _optionsMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="optionsMonitor">The options monitor for request and response logging.</param>
    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger,
        IOptionsMonitor<RequestResponseLoggingOptions> optionsMonitor)
    {
        _next = next;
        _logger = logger;
        _optionsMonitor = optionsMonitor;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldLogRequest())
        {
            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBody(context.Request);

            _logger.LogDebug(
                "Http Request Information: {Method} {Path} {QueryString} {RequestBody}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                requestBody);
        }

        if (ShouldLogResponse())
        {
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            finally
            {
                var responseContent = await ReadResponseBody(context.Response);
                _logger.LogDebug(
                    "Http Response Information: {StatusCode} {ResponseBody}",
                    context.Response.StatusCode,
                    responseContent);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        else
        {
            await _next(context);
        }

        bool ShouldLogRequest() =>
            _optionsMonitor.CurrentValue.EnabledRequestLog && _logger.IsEnabled(LogLevel.Debug);

        bool ShouldLogResponse() =>
            _optionsMonitor.CurrentValue.EnabledResponseLog && _logger.IsEnabled(LogLevel.Debug);
    }

    /// <summary>
    /// Reads the request body as a string.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The request body as a string.</returns>
    private static async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.Body.Position = 0;

        using var reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    /// <summary>
    /// Reads the response body as a string.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <returns>The response body as a string.</returns>
    private static async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(
            response.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var text = await reader.ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }
}
