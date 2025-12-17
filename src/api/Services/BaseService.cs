using Example.Api.Dtos.Responses;
using Example.Api.Enums;

namespace Example.Api.Services;

/// <summary>
/// Base class for services.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Creates a successful API result.
    /// </summary>
    /// <param name="message">Optional success message.</param>
    /// <returns>A successful ApiResult instance.</returns>
    protected ApiResult SuccessResult(string? message = default)
    {
        return new ApiResult
        {
            Success = true,
            Code = ApiCode.Success,
            Message = message,
        };
    }

    /// <summary>
    /// Creates a successful API result with data.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="message">Optional success message.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A successful ApiResult instance containing the data.</returns>
    protected ApiResult<T> SuccessResult<T>(T data, string? message = default)
    {
        return new ApiResult<T>
        {
            Success = true,
            Code = ApiCode.Success,
            Message = message,
            Data = data,
        };
    }

    /// <summary>
    /// Creates a successful paged API result with data.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalCount">The total number of items.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A successful ApiResult instance containing the paged data.</returns>
    protected ApiResult<PagedResult<T>> SuccessPagedResult<T>(
        IEnumerable<T> data,
        int pageNumber,
        int pageSize,
        long totalCount)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pagedResult = new PagedResult<T>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
        };

        return new ApiResult<PagedResult<T>>
        {
            Success = true,
            Code = ApiCode.Success,
            Data = pagedResult,
        };
    }

    /// <summary>
    /// Creates a bad request API result.
    /// </summary>
    /// <param name="message">Optional error message.</param>
    /// <returns>A bad request ApiResult instance.</returns>
    protected ApiResult BadRequestResult(string? message = default)
    {
        return new ApiResult
        {
            Success = false,
            Code = ApiCode.InvalidRequest,
            Message = message,
        };
    }

    /// <summary>
    /// Creates a bad request API result with data.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="message">Optional error message.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A bad request ApiResult instance containing the data.</returns>
    protected ApiResult<T> BadRequestResult<T>(T data, string? message = default)
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = ApiCode.InvalidRequest,
            Message = message,
            Data = data,
        };
    }

    /// <summary>
    /// Creates a failure API result.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">Optional error message.</param>
    /// <returns>A failure ApiResult instance.</returns>
    protected ApiResult FailureResult(ApiCode code, string? message = default)
    {
        return new ApiResult
        {
            Success = false,
            Code = code,
            Message = message,
        };
    }

    /// <summary>
    /// Creates a failure API result with data.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="message">Optional error message.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A failure ApiResult instance containing the data.</returns>
    protected ApiResult<T> FailureResult<T>(ApiCode code, T data, string? message = default)
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = code,
            Message = message,
            Data = data,
        };
    }

    /// <summary>
    /// Creates a no data found API result.
    /// </summary>
    /// <param name="message">Optional message indicating no data was found.</param>
    /// <returns>A no data found ApiResult instance.</returns>
    protected ApiResult NoDataFoundResult(string? message = default)
    {
        return new ApiResult
        {
            Success = true,
            Code = ApiCode.NoDataFound,
            Message = message ?? "No data found.",
        };
    }

    /// <summary>
    /// Creates a no data found API result with data.
    /// </summary>
    /// <param name="message">Optional message indicating no data was found.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A no data found ApiResult instance containing the data.</returns>
    protected ApiResult<T> NoDataFoundResult<T>(string? message = default)
    {
        return new ApiResult<T>
        {
            Success = true,
            Code = ApiCode.NoDataFound,
            Message = message ?? "No data found.",
            Data = default,
        };
    }

    /// <summary>
    /// Creates a no data found API result with paged data.
    /// </summary>
    /// <param name="message">Optional message indicating no data was found.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>A no data found ApiResult instance containing the paged data.</returns>
    protected ApiResult<PagedResult<T>> NoDataFoundPagedResult<T>(string? message = default)
    {
        return new ApiResult<PagedResult<T>>
        {
            Success = true,
            Code = ApiCode.NoDataFound,
            Message = message ?? "No data found.",
            Data = new PagedResult<T>
            {
                Data = [],
                PageNumber = 1,
                PageSize = 0,
                TotalCount = 0,
                TotalPages = 0,
            },
        };
    }

    /// <summary>
    /// Creates an error API result.
    /// </summary>
    /// <param name="message">Optional error message.</param>
    /// <returns>An error ApiResult instance.</returns>
    protected ApiResult ErrorResult(string? message = default)
    {
        return new ApiResult
        {
            Success = false,
            Code = ApiCode.UnknownError,
            Message = message,
        };
    }

    /// <summary>
    /// Creates an error API result with data.
    /// </summary>
    /// <param name="message">Optional error message.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>An error ApiResult instance containing the data.</returns>
    protected ApiResult<T> ErrorResult<T>(string? message = default)
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = ApiCode.UnknownError,
            Message = message,
            Data = default,
        };
    }

    /// <summary>
    /// Creates an error API result with data.
    /// </summary>
    /// <param name="data">The data to include in the result.</param>
    /// <param name="message">Optional error message.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>An error ApiResult instance containing the data.</returns>
    protected ApiResult<T> ErrorResult<T>(T data, string? message = default)
    {
        return new ApiResult<T>
        {
            Success = false,
            Code = ApiCode.UnknownError,
            Message = message,
            Data = data,
        };
    }
}
