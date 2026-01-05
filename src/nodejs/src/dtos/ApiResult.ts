import { ApiCode } from "./ApiCode";

/**
 * Base class for API responses.
 */
export class ApiResult {
  /** Indicates whether the operation was successful. */
  success: boolean;
  /** The API response code. */
  code: ApiCode;
  /** A descriptive message about the operation result. */
  message: string;

  /**
   * Creates a new API result.
   * @param success - Whether the operation was successful.
   * @param code - The API response code.
   * @param message - A descriptive message about the operation result.
   */
  constructor(success: boolean, code: ApiCode, message: string = "") {
    this.success = success;
    this.code = code;
    this.message = message;
  }
}

/**
 * API response containing data of type T.
 * @template T - The type of data contained in the response.
 */
export class ApiDataResult<T> extends ApiResult {
  /** The data returned by the API operation. */
  data?: T;

  /**
   * Creates a new API data result.
   * @param success - Whether the operation was successful.
   * @param code - The API response code.
   * @param message - A descriptive message about the operation result.
   * @param data - The data returned by the operation.
   */
  constructor(success: boolean, code: ApiCode, message: string = "", data?: T) {
    super(success, code, message);
    this.data = data;
  }
}
