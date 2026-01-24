using Example.Api.Dtos.Responses;
using Example.Api.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Example.Api.Extensions;

/// <summary>
/// Extension methods for ApiResult to convert to IResult.
/// </summary>
public static class ApiResultExtensions
{
    /// <summary>
    /// Executes an asynchronous action if the ApiResult is successful, returning the original ApiResult.
    /// </summary>
    /// <param name="sourceResult">The source ApiResult task.</param>
    /// <param name="onSuccessHandler">The asynchronous action to perform if the result is successful.</param>
    /// <typeparam name="T">The type of the ApiResult.</typeparam>
    /// <returns>A task representing the asynchronous operation, containing the original ApiResult.</returns>
    public static async ValueTask<ApiResult<T>> TapOnSuccessAsync<T>(
        this Task<ApiResult<T>> sourceResult,
        Func<ValueTask> onSuccessHandler)
    {
        var result = await sourceResult;

        if (!result.Success)
        {
            return result;
        }

        await onSuccessHandler();
        return result;
    }

    /// <summary>
    /// Executes an asynchronous action if the ApiResult is successful, returning the original ApiResult.
    /// </summary>
    /// <param name="sourceResult">The source ApiResult task.</param>
    /// <param name="onSuccessHandler">The asynchronous action to perform if the result is successful.</param>
    /// <typeparam name="T">The type of the ApiResult.</typeparam>
    /// <returns>A task representing the asynchronous operation, containing the original ApiResult.</returns>
    public static async ValueTask<ApiResult<T>> TapOnSuccessAsync<T>(
        this Task<ApiResult<T>> sourceResult,
        Func<ApiResult<T>, ValueTask> onSuccessHandler)
    {
        var result = await sourceResult;

        if (!result.Success)
        {
            return result;
        }

        await onSuccessHandler(result);
        return result;
    }

    /// <summary>
    /// Converts an ApiResult to an appropriate IResult for HTTP responses.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Results<Ok<ApiResult>, BadRequest<ApiResult>, InternalServerError<ApiResult>, StatusCodeHttpResult> 
        ToHttpResult(this ApiResult result)
    {
        return result.Code switch
        {
            ApiCode.Success => TypedResults.Ok(result),
            ApiCode.InvalidRequest => TypedResults.BadRequest(result),
            ApiCode.NoDataFound => TypedResults.Ok(result),
            ApiCode.DataAccessError => TypedResults.InternalServerError(result),
            ApiCode.OperationFailed => TypedResults.Ok(result),
            ApiCode.OperationTimeout => TypedResults.StatusCode(StatusCodes.Status504GatewayTimeout),
            ApiCode.UnknownError => TypedResults.InternalServerError(result),
            _ => TypedResults.InternalServerError(result)
        };
    }
}
