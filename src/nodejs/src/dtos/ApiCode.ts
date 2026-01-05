/**
 * Enum representing standard API response codes.
 */
export enum ApiCode {
  /** Operation completed successfully. */
  Success = 1,
  /** An unknown error occurred. */
  UnknownError = 2,
  /** The request was invalid or malformed. */
  InvalidRequest = 3,
  /** No data was found for the requested resource. */
  NoDataFound = 4,
  /** An error occurred while accessing the data store. */
  DataAccessError = 5,
  /** The operation failed to complete. */
  OperationFailed = 6,
  /** The operation timed out. */
  OperationTimeout = 7,
}
