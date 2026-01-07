"""Global exception handlers for FastAPI application."""

import logging
from fastapi import FastAPI, Request, status
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

logger = logging.getLogger(__name__)


def setup_exception_handlers(app: FastAPI) -> None:
    """
    Register global exception handlers to the FastAPI application.

    Args:
        app: FastAPI application instance
    """

    @app.exception_handler(Exception)
    async def global_exception_handler(request: Request, exc: Exception):
        """Handle all unhandled exceptions."""
        logger.error(
            "Unhandled exception occurred: %s, Path: %s, Method: %s",
            str(exc),
            request.url.path,
            request.method,
            exc_info=True,
        )
        return JSONResponse(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            content={
                "success": False,
                "code": "INTERNAL_SERVER_ERROR",
                "message": "An internal server error occurred. Please contact support.",
                "data": None,
            },
        )

    @app.exception_handler(RequestValidationError)
    async def validation_exception_handler(request: Request, exc: RequestValidationError):
        """Handle request validation errors."""
        logger.warning(
            "Request validation error: %s, Path: %s, Method: %s, Errors: %s",
            str(exc),
            request.url.path,
            request.method,
            exc.errors(),
        )
        return JSONResponse(
            status_code=status.HTTP_422_UNPROCESSABLE_ENTITY,
            content={
                "success": False,
                "code": "VALIDATION_ERROR",
                "message": "Request validation failed",
                "data": {"errors": exc.errors()},
            },
        )
