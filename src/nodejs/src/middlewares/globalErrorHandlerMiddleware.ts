import { ApiCode } from "../dtos/ApiCode";
import { ApiResult } from "../dtos/ApiResult";
import { Request, Response, NextFunction } from "express";
import logger from "../utils/logger";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const errorHandler = (err: any, req: Request, res: Response, _next: NextFunction) => {
  const statusCode = err.statusCode || 500;
  const message = err.message || "Internal Server Error";

  logger.error(`Error processing request ${req.method} ${req.url}`, {
    error: {
      name: err.name,
      message: err.message,
      stack: err.stack,
      code: err.code,
      statusCode: err.statusCode,
    },
    body: req.body,
    query: req.query,
    params: req.params,
    ip: req.ip,
  });

  const response = new ApiResult(false, ApiCode.UnknownError, message);

  if (process.env.NODE_ENV === "development") {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    (response as any).stack = err.stack;
  }

  res.status(statusCode).json(response);
};
