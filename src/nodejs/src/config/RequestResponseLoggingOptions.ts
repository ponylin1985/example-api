/**
 * Options for request and response logging middleware.
 */
export interface IRequestResponseLoggingOptions {
  /**
   * Enable request body logging.
   */
  enabledRequestLog: boolean;

  /**
   * Enable response body logging.
   */
  enabledResponseLog: boolean;
}

/**
 * Default options for request and response logging.
 */
export const defaultRequestResponseLoggingOptions: IRequestResponseLoggingOptions = {
  enabledRequestLog: process.env.LOG_REQUEST_BODY === "true",
  enabledResponseLog: process.env.LOG_RESPONSE_BODY === "true",
};
