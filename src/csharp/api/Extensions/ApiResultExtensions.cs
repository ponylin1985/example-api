using Example.Api.Dtos.Responses;
using Example.Api.Enums;

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
    /// Converts an ApiResult to the appropriate HTTP IResult based on the ApiCode.
    /// </summary>
    /// <param name="result">The ApiResult to convert.</param>
    /// <returns>The corresponding IResult.</returns>
    public static IResult ToHttpResult(this ApiResult result)
    {
        return result.Code switch
        {
            ApiCode.Success => Results.Ok(result),
            ApiCode.InvalidRequest => Results.BadRequest(result),
            ApiCode.NoDataFound => Results.Ok(result),
            ApiCode.DataAccessError => Results.InternalServerError(result),
            ApiCode.OperationFailed => Results.Ok(result),
            ApiCode.OperationTimeout => Results.StatusCode(StatusCodes.Status504GatewayTimeout),
            ApiCode.UnknownError => Results.InternalServerError(result),
            _ => Results.InternalServerError(result),
        };
    }
}
