"""Redis client dependency injection."""

import redis.asyncio as redis
from app.configs.cache_config import cache_settings

_redis_client: redis.Redis | None = None


async def get_redis() -> redis.Redis:
    """
    Get Redis client instance.

    Returns:
        Redis client for caching operations
    """
    global _redis_client
    if _redis_client is None:
        _redis_client = redis.Redis.from_url(
            cache_settings.redis_url, encoding="utf-8", decode_responses=False  # Keep as bytes for better control
        )
    return _redis_client


async def close_redis():
    """Close Redis connection pool."""
    global _redis_client
    if _redis_client is not None:
        await _redis_client.close()
        _redis_client = None
