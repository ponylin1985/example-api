using Example.Api.Enums;

namespace Example.Api.Dtos.Responses;

public record ApiResult
{
    public bool Success { get; init; }
    public ApiCode Code { get; init; }
    public string? Message { get; init; } = string.Empty;
}

public record ApiResult<T> : ApiResult
{
    public T? Data { get; init; }
}
