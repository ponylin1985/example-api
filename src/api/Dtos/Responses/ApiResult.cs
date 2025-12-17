using Example.Api.Enums;

namespace Example.Api.Dtos.Responses;

/// <summary>
/// Represents the result of an API or Service operation.
/// </summary>
/// <value></value>
public record ApiResult
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    /// <value></value>
    public bool Success { get; init; }

    /// <summary>
    /// The code representing the result of the operation.
    /// </summary>
    /// <value></value>
    public ApiCode Code { get; init; }

    /// <summary>
    /// The message providing additional information about the result.
    /// </summary>
    /// <value></value>
    public string? Message { get; init; } = string.Empty;
}

/// <summary>
/// Represents the result of an API or Service operation with data.
/// </summary>
/// <value></value>
public record ApiResult<T> : ApiResult
{
    /// <summary>
    /// The data returned by the operation.
    /// </summary>
    /// <value></value>
    public T? Data { get; init; }
}
