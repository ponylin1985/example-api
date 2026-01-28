using Example.Api.Enums;

namespace Example.Api.Infrastructure;

/// <summary>
/// Represents errors that occur during business logic execution.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Gets the error code associated with the business exception.
    /// </summary>
    /// <value></value>
    public ApiCode ErrorCode { get; }
    
    /// <summary>
    /// Constructor for BusinessException.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    /// <param name="message">Error message.</param>
    /// <returns></returns>
    public BusinessException(ApiCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Gets the stack trace associated with the business exception.<para/>
    /// This is overridden to return an empty string to avoid exposing internal stack traces for better performance and security.
    /// </summary>
    public override string? StackTrace => string.Empty;
}
