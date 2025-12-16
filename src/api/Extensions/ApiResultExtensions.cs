using Example.Api.Dtos.Responses;
using Example.Api.Enums;

namespace Example.Api.Extensions;

/// <summary>
/// Extension methods for ApiResult to convert to IResult.
/// </summary>
public static class ApiResultExtensions
{
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
            ApiCode.NoDataFound => Results.NotFound(result),
            ApiCode.UnknownError => Results.InternalServerError(result),
            _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
