import { ApiCode } from "../dtos/ApiCode";
import { ApiDataResult, ApiResult } from "../dtos/ApiResult";
import { PagedResult } from "../dtos/PagedResult";

/**
 * Base class for services providing common helper methods.
 */
export abstract class BaseService {
  /**
   * Creates a successful API result.
   * @param message Optional success message.
   * @returns A successful ApiResult instance.
   */
  protected successResult(message?: string): ApiResult {
    return new ApiResult(true, ApiCode.Success, message || "Success");
  }

  /**
   * Creates a successful API result with data.
   * @param data The data to include in the result.
   * @param message Optional success message.
   * @returns A successful ApiDataResult instance containing the data.
   */
  protected successDataResult<T>(data: T, message?: string): ApiDataResult<T> {
    return new ApiDataResult<T>(true, ApiCode.Success, message || "Success", data);
  }

  /**
   * Creates a successful paged API result with data.
   * @param data The data to include in the result.
   * @param totalCount The total number of items.
   * @param pageNumber The current page number.
   * @param pageSize The number of items per page.
   * @param message Optional success message.
   * @returns A successful ApiDataResult instance containing the paged data.
   */
  protected successPagedResult<T>(
    data: T[],
    totalCount: number,
    pageNumber: number,
    pageSize: number,
    message?: string
  ): ApiDataResult<PagedResult<T>> {
    const pagedResult = new PagedResult(data, totalCount, pageNumber, pageSize);
    return new ApiDataResult<PagedResult<T>>(true, ApiCode.Success, message || "Success", pagedResult);
  }

  /**
   * Creates a bad request API result.
   * @param message Optional error message.
   * @returns A bad request ApiResult instance.
   */
  protected badRequestResult(message?: string): ApiResult {
    return new ApiResult(false, ApiCode.InvalidRequest, message || "Bad request");
  }

  /**
   * Creates a bad request API result with data.
   * @param message Optional error message.
   * @returns A bad request ApiDataResult instance.
   */
  protected badRequestDefaultDataResult<T>(message?: string): ApiDataResult<T> {
    return new ApiDataResult<T>(false, ApiCode.InvalidRequest, message || "Bad request", undefined);
  }

  /**
   * Creates a bad request API result with data.
   * @param data Optional data to include in the result.
   * @param message Optional error message.
   * @returns A bad request ApiDataResult instance.
   */
  protected badRequestDataResult<T>(data?: T, message?: string): ApiDataResult<T> {
    return new ApiDataResult<T>(false, ApiCode.InvalidRequest, message || "Bad request", data);
  }

  /**
   * Creates a failure API result.
   * @param code The error code.
   * @param message Optional error message.
   * @returns A failure ApiResult instance.
   */
  protected failureResult(code: ApiCode, message?: string): ApiResult {
    return new ApiResult(false, code, message || "Operation failed");
  }

  /**
   * Creates a failure API result with data.
   * @param code The error code.
   * @param data Optional data to include in the result.
   * @param message Optional error message.
   * @returns A failure ApiDataResult instance.
   */
  protected failureDataResult<T>(code: ApiCode, data?: T, message?: string): ApiDataResult<T> {
    return new ApiDataResult<T>(false, code, message || "Operation failed", data);
  }

  /**
   * Creates a no data found API result.
   * @param message Optional message indicating no data was found.
   * @returns A no data found ApiResult instance.
   */
  protected noDataFoundResult(message?: string): ApiResult {
    return new ApiResult(true, ApiCode.NoDataFound, message || "No data found");
  }

  /**
   * Creates a no data found API result with data.
   * @param data Optional data to include in the result.
   * @param message Optional message indicating no data was found.
   * @returns A no data found ApiDataResult instance.
   */
  protected noDataFoundDataResult<T>(data?: T, message?: string): ApiDataResult<T> {
    return new ApiDataResult<T>(true, ApiCode.NoDataFound, message || "No data found", data);
  }

  /**
   * Creates a no data found paged API result.
   * @param message Optional message indicating no data was found.
   * @returns A no data found ApiDataResult instance with empty paged result.
   */
  protected noDataFoundPagedResult<T>(message?: string): ApiDataResult<PagedResult<T>> {
    const emptyPagedResult = new PagedResult<T>([], 0, 1, 0);
    return new ApiDataResult<PagedResult<T>>(true, ApiCode.NoDataFound, message || "No data found", emptyPagedResult);
  }

  /**
   * Creates an error API result.
   * @param message Optional error message.
   * @returns An error ApiResult instance.
   */
  protected errorResult(message?: string): ApiResult {
    return new ApiResult(false, ApiCode.UnknownError, message || "An error occurred");
  }

  /**
   * Creates an error API result with data.
   * @param data Optional data to include in the result.
   * @param message Optional error message.
   * @returns An error ApiDataResult instance.
   */
  protected errorDataResult<T>(data?: T, message?: string): ApiDataResult<T> {
    return new ApiDataResult<T>(false, ApiCode.UnknownError, message || "An error occurred", data);
  }
}
