"""Main FastAPI application."""

import logging
from contextlib import asynccontextmanager
from fastapi import FastAPI
from fastapi.responses import JSONResponse

from app.config import settings
from app.api import patient_endpoints, order_endpoints

# Configure logging
logging.basicConfig(level=settings.log_level, format="%(asctime)s - %(name)s - %(levelname)s - %(message)s")
logger = logging.getLogger(__name__)


@asynccontextmanager
async def lifespan(application: FastAPI):
    """Lifespan context manager for startup and shutdown events."""
    logger.info("Starting up FastAPI application...")
    yield
    logger.info("Shutting down FastAPI application...")


# Create FastAPI application
app = FastAPI(
    title="Example API - Python FastAPI",
    docs_url="/swagger",
    description="Multi-language implementation of Example API - Python FastAPI version",
    version="1.0.0",
    lifespan=lifespan,
)


@app.get("/healthz", tags=["Health"])
async def health_check():
    """
    Health check endpoint.

    Matches C# endpoint: GET /healthz
    """
    return JSONResponse(content={"status": "healthy"}, status_code=200)


# Include routers
app.include_router(patient_endpoints.router)
app.include_router(order_endpoints.router)


if __name__ == "__main__":
    import uvicorn

    uvicorn.run(
        "main:app",
        host=settings.server_host,
        port=settings.server_port,
        reload=True,
        log_level=settings.log_level.lower(),
    )
