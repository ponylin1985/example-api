using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Example.Api.Infrastructure;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Example.Api.Middlewares;

/// <summary>
/// Middleware for handling global exceptions.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    /// <summary>
    /// The next middleware in the pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger for the GlobalExceptionHandlerMiddleware.
    /// </summary>
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// JSON serializer options for consistent API responses.
    /// </summary>
    /// <returns></returns>
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        AllowTrailingCommas = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger.</param>
    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions that occur during the http request pipeline.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var apiResult = exception switch
        {
            BusinessException bizEx => CreateFromBusinessException(context, bizEx),
            _ => CreateFromUnhandledException(context, exception)
        };

        await context.Response.WriteAsJsonAsync(apiResult, _jsonOptions);
    }

    /// <summary>
    /// Creates an ApiResult from a BusinessException.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    private ApiResult CreateFromBusinessException(HttpContext context, BusinessException ex)
    {
        _logger.LogWarning(ex, "Operation failure due to business rule violation: {Message}", ex.Message);

        context.Response.StatusCode = ex.ErrorCode switch
        {
            ApiCode.Success or ApiCode.NoDataFound or ApiCode.OperationFailed => StatusCodes.Status200OK,
            ApiCode.InvalidRequest => StatusCodes.Status400BadRequest,
            ApiCode.DataAccessError or ApiCode.UnknownError => StatusCodes.Status500InternalServerError,
            ApiCode.OperationTimeout => StatusCodes.Status504GatewayTimeout,
            _ => StatusCodes.Status500InternalServerError,
        };

        return new ApiResult
        {
            Success = false,
            Code = ex.ErrorCode,
            Message = ex.Message,
        };
    }

    /// <summary>
    /// Creates an ApiResult from an unhandled exception.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    private ApiResult CreateFromUnhandledException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled System Error.");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return new ApiResult
        {
            Success = false,
            Code = ApiCode.UnknownError,
            Message = "An internal server error occurred. Please try again later."
        };
    }
}
