import { Request, Response, NextFunction } from "express";
import logger from "../utils/logger";

export const requestLogger = (req: Request, res: Response, next: NextFunction) => {
  const start = Date.now();

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
