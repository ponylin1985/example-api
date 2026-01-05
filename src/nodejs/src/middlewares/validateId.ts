import { Request, Response, NextFunction } from "express";

/**
 * Middleware to validate that the route parameter 'id' is a positive integer.
 * @param req - Express request object
 * @param res - Express response object
 * @param next - Express next function
 */
export function validateId(req: Request, res: Response, next: NextFunction): void {
  const id = parseInt(req.params.id);

  if (isNaN(id) || id <= 0 || !Number.isInteger(id)) {
    res.status(400).json({
      success: false,
      message: "Invalid ID: must be a positive integer",
    });
    return;
  }

  next();
}
