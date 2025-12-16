using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Api.Dtos.Responses;
using Example.Api.Enums;

namespace Example.Api.Services;

/// <summary>
/// Base class for services.
/// </summary>
public abstract class BaseService
{
    protected ApiResult SuccessResult(string? message = default)
    {
        return new ApiResult
        {
            Success = true,
            Code = ApiCode.Success,
            Message = message,
        };
    }

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

    protected ApiResult BadRequestResult(string? message = default)
    {
        return new ApiResult
        {
            Success = false,
            Code = ApiCode.InvalidRequest,
            Message = message,
        };
    }

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

    protected ApiResult FailureResult(ApiCode code, string? message = default)
    {
        return new ApiResult
        {
            Success = false,
            Code = code,
            Message = message,
        };
    }

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

    protected ApiResult NoDataFoundResult(string? message = default)
    {
        return new ApiResult
        {
            Success = true,
            Code = ApiCode.NoDataFound,
            Message = message ?? "No data found.",
        };
    }

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

    protected ApiResult ErrorResult(string? message = default)
    {
        return new ApiResult
        {
            Success = false,
            Code = ApiCode.UnknownError,
            Message = message,
        };
    }

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
