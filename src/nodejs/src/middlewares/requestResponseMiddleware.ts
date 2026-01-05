import { Request, Response, NextFunction } from "express";
import logger from "../utils/logger";
import {
  IRequestResponseLoggingOptions,
  defaultRequestResponseLoggingOptions,
} from "../config/RequestResponseLoggingOptions";

/**
 * Reads the request body as a string.
 * @param req - The HTTP request.
 * @returns The request body as a string.
 */
const readRequestBody = (req: Request): string => {
  if (req.body) {
    return JSON.stringify(req.body);
  }
  return "";
};

/**
 * Creates a request logger middleware with optional configuration.
 * @param options - Optional logging configuration.
 * @returns Express middleware function.
 */
export const createRequestLogger = (options: IRequestResponseLoggingOptions = defaultRequestResponseLoggingOptions) => {
  return (req: Request, res: Response, next: NextFunction) => {
    const start = Date.now();

    // Log request if enabled
    if (options.enabledRequestLog) {
      const requestBody = readRequestBody(req);
      logger.debug("Http Request Information", {
        method: req.method,
        path: req.path,
        queryString: req.query,
        requestBody,
      });
    }

    // Log response if enabled
    if (options.enabledResponseLog) {
      const originalSend = res.send;
      const originalJson = res.json;

      // Override res.send to capture response body
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      res.send = function (body: any): Response {
        const duration = Date.now() - start;
        logger.debug("Http Response Information", {
          statusCode: res.statusCode,
          duration: `${duration}ms`,
          responseBody: typeof body === "string" ? body : JSON.stringify(body),
        });
        return originalSend.call(this, body);
      };

      // Override res.json to capture JSON response body
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      res.json = function (body: any): Response {
        const duration = Date.now() - start;
        logger.debug("Http Response Information", {
          statusCode: res.statusCode,
          duration: `${duration}ms`,
          responseBody: JSON.stringify(body),
        });
        return originalJson.call(this, body);
      };
    }

    // Log basic request/response info on finish
    res.on("finish", () => {
      const duration = Date.now() - start;
      const message = `${req.method} ${req.originalUrl} ${res.statusCode} ${duration}ms`;

      if (res.statusCode >= 400) {
        logger.warn(message, {
          method: req.method,
          url: req.originalUrl,
          status: res.statusCode,
          duration,
          ip: req.ip,
        });
      } else {
        logger.info(message, {
          method: req.method,
          url: req.originalUrl,
          status: res.statusCode,
          duration,
          ip: req.ip,
        });
      }
    });

    next();
  };
};

/**
 * Default request logger middleware instance.
 */
export const requestLogger = createRequestLogger();
