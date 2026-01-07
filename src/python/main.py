"""Main FastAPI application."""

import logging
from app.configs.database_config import settings
from app.configs.logging_config import setup_logging
from app.handlers import setup_exception_handlers
from app.infrastructure.redis_client import close_redis
from app.middlewares import setup_http_logging_middleware
from app.routers import patients, orders
from contextlib import asynccontextmanager
from fastapi import FastAPI
from fastapi.responses import JSONResponse

setup_logging()
logger = logging.getLogger(__name__)


@asynccontextmanager
async def lifespan(application: FastAPI):
    """Lifespan context manager for startup and shutdown events."""
    logger.info("Starting up FastAPI application...")
    yield
    logger.info("Shutting down FastAPI application...")
    await close_redis()
    logger.info("Redis connection closed")


app = FastAPI(
    title="Example API - Python FastAPI",
    docs_url="/swagger",
    description="Multi-language implementation of Example API - Python FastAPI version",
    version="1.0.0",
    lifespan=lifespan,
)

setup_http_logging_middleware(app, max_body_size=50000)
setup_exception_handlers(app)


@app.get("/healthz", tags=["Health"])
async def health_check():
    """
    Health check endpoint.
    """
    return JSONResponse(content={"status": "ok"}, status_code=200)


app.include_router(patients.router)
app.include_router(orders.router)


if __name__ == "__main__":
    import uvicorn

    uvicorn.run(
        "main:app",
        host=settings.server_host,
        port=settings.server_port,
        reload=True,
        log_level=settings.log_level.lower(),
    )
