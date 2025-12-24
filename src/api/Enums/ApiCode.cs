namespace Example.Api.Enums;

/// <summary>
/// API response codes.
/// </summary>
public enum ApiCode : byte
{
    /// <summary>
    /// Operation was successful.
    /// </summary>
    Success = 1,

    /// <summary>
    /// An unknown error occurred.
    /// </summary>
    UnknownError = 2,

    /// <summary>
    /// The request was invalid.
    /// </summary>
    InvalidRequest = 3,

    /// <summary>
    /// No data was found for the request.
    /// </summary>
    NoDataFound = 4,

    /// <summary>
    /// A data access error occurred.
    /// </summary>
    DataAccessError = 5,

    /// <summary>
    /// The operation failed.
    /// </summary>
    OperationFailed = 6,

    /// <summary>
    /// The operation timedout.
    /// </summary>
    OperationTimeout = 7,
}
