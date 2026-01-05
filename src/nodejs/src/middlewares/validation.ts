import { plainToInstance } from "class-transformer";
import { validate, ValidationError } from "class-validator";
import { Request, Response, NextFunction } from "express";
import { ApiResult } from "../dtos/ApiResult";
import { ApiCode } from "../dtos/ApiCode";

/**
 * Type for class constructor.
 */
// eslint-disable-next-line @typescript-eslint/no-explicit-any
type ClassConstructor<T = any> = new (...args: any[]) => T;

/**
 * Middleware for validating request body against a DTO class.
 * @param dtoClass - The DTO class to validate against.
 * @returns Express middleware function.
 */
export function validateBody<T>(dtoClass: ClassConstructor<T>) {
  return async (req: Request, res: Response, next: NextFunction) => {
    const dtoInstance = plainToInstance(dtoClass, req.body);
    const errors: ValidationError[] = await validate(dtoInstance as object);

    if (errors.length > 0) {
      const errorMessage = errors.map((error) => Object.values(error.constraints || {}).join(", ")).join("; ");
      const result = new ApiResult(false, ApiCode.InvalidRequest, errorMessage);
      res.status(400).json(result);
      return;
    }

    req.body = dtoInstance;
    next();
  };
}
