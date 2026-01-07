"""HTTP logging middleware for FastAPI application."""

import logging
import time
import json
from fastapi import Request, Response
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.types import Message
from typing import Callable

logger = logging.getLogger(__name__)


class HTTPLoggingMiddleware(BaseHTTPMiddleware):
    """Middleware to log all HTTP requests and responses."""

    def __init__(self, app, max_body_size: int = 10000):
        """
        Initialize the middleware.

        Args:
            app: FastAPI application
            max_body_size: Maximum body size to log in bytes (default: 10KB)
        """
        super().__init__(app)
        self.max_body_size = max_body_size

    async def set_body(self, request: Request):
        """Read and cache request body."""
        receive_ = await request._receive()

        async def receive() -> Message:
            return receive_

        request._receive = receive

    async def dispatch(self, request: Request, call_next: Callable) -> Response:
        """
        Process the request and log details.

        Args:
            request: Incoming request
            call_next: Next middleware/handler in chain

        Returns:
            Response from the application
        """
        # Start timing
        start_time = time.time()

        method = request.method
        url = str(request.url)
        path = request.url.path
        query_params = dict(request.query_params)
        headers = dict(request.headers)
        client_host = request.client.host if request.client else "unknown"

        request_body = None
        if method in ["POST", "PUT", "PATCH"]:
            try:
                body_bytes = await request.body()
                if len(body_bytes) <= self.max_body_size:
                    request_body = body_bytes.decode("utf-8")
                    # Try to parse as JSON for better readability
                    try:
                        request_body = json.loads(request_body)
                    except json.JSONDecodeError:
                        pass  # Keep as string if not JSON
                else:
                    request_body = f"<Body too large: {len(body_bytes)} bytes>"
            except Exception as e:
                request_body = f"<Error reading body: {str(e)}>"

        # Log request
        logger.info(
            "HTTP Request - Method: %s, Path: %s, Client: %s, Query: %s",
            method,
            path,
            client_host,
            query_params if query_params else "None",
        )

        if request_body:
            logger.debug("Request Body: %s", request_body)

        try:
            response = await call_next(request)
            process_time = time.time() - start_time

            response_body = None
            response_body_bytes = b""
            async for chunk in response.body_iterator:
                response_body_bytes += chunk

            if len(response_body_bytes) <= self.max_body_size:
                try:
                    response_body = response_body_bytes.decode("utf-8")
                    # Try to parse as JSON
                    try:
                        response_body = json.loads(response_body)
                    except json.JSONDecodeError:
                        pass
                except Exception:
                    response_body = f"<Binary data: {len(response_body_bytes)} bytes>"
            else:
                response_body = f"<Response too large: {len(response_body_bytes)} bytes>"

            logger.info(
                "HTTP Response - Status: %s, Path: %s, Duration: %.3fs",
                response.status_code,
                path,
                process_time,
            )

            if response_body:
                logger.debug("Response Body: %s", response_body)

            return Response(
                content=response_body_bytes,
                status_code=response.status_code,
                headers=dict(response.headers),
                media_type=response.media_type,
            )

        except Exception as e:
            # Log error
            process_time = time.time() - start_time
            logger.error(
                "HTTP Request Failed - Method: %s, Path: %s, Duration: %.3fs, Error: %s",
                method,
                path,
                process_time,
                str(e),
                exc_info=True,
            )
            raise


def setup_http_logging_middleware(app, max_body_size: int = 10000) -> None:
    """
    Add HTTP logging middleware to the FastAPI application.

    Args:
        app: FastAPI application instance
        max_body_size: Maximum body size to log in bytes (default: 10KB)
    """
    app.add_middleware(HTTPLoggingMiddleware, max_body_size=max_body_size)
    logger.info("HTTP logging middleware enabled (max body size: %d bytes)", max_body_size)
