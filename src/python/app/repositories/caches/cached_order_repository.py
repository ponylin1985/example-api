"""Cached order repository with Redis decorator."""

import asyncio
import json
import logging
from datetime import datetime
from typing import Optional
import redis.asyncio as redis
from app.entities import PatientOrder
from app.repositories.order_repository import OrderRepository
from app.configs.cache_config import cache_settings

logger = logging.getLogger(__name__)


class CachedOrderRepository:
    """Decorator for OrderRepository that adds Redis caching."""

    def __init__(self, inner_repository: OrderRepository, redis_client: redis.Redis):
        """
        Initialize cached repository.

        Args:
            inner_repository: The inner order repository to which calls are delegated
            redis_client: Redis client for caching
        """
        self._inner_repository = inner_repository
        self._redis = redis_client
        self._ttl = cache_settings.cache_ttl_seconds

    @staticmethod
    def _get_order_cache_key(order_id: int) -> str:
        """Get cache key for an order."""
        return f"order:{order_id}"

    @staticmethod
    def _get_patient_cache_key(patient_id: int) -> str:
        """Get cache key for a patient."""
        return f"patient:{patient_id}"

    async def _save_to_cache(self, order: PatientOrder) -> None:
        """Save order to cache."""
        try:
            key = self._get_order_cache_key(order.id)
            order_dict = {
                "id": order.id,
                "message": order.message,
                "patient_id": order.patient_id,
                "created_at": order.created_at.isoformat(),
                "updated_at": order.updated_at.isoformat(),
            }
            await self._redis.setex(key, self._ttl, json.dumps(order_dict))
            logger.debug("Saved order %s to cache", order.id)
        except Exception as e:
            logger.error("Error occurred while saving order to cache with key %s after retries: %s", key, str(e))

    async def _remove_from_cache(self, order_id: int, patient_id: int) -> None:
        """Remove order and associated patient from cache."""
        try:
            tasks = []

            if order_id > 0:
                order_cache_key = self._get_order_cache_key(order_id)
                tasks.append(self._redis.delete(order_cache_key))

            if patient_id > 0:
                patient_cache_key = self._get_patient_cache_key(patient_id)
                tasks.append(self._redis.delete(patient_cache_key))

            if tasks:
                await asyncio.gather(*tasks)

            logger.debug("Removed cache for order %s and patient %s", order_id, patient_id)
        except Exception as e:
            logger.error(
                "Error occurred while removing cache for Order %s or Patient %s after retries: %s",
                order_id,
                patient_id,
                str(e),
            )

    async def get_order(self, order_id: int) -> Optional[PatientOrder]:
        """Get order by ID (with caching)."""
        key = self._get_order_cache_key(order_id)

        try:
            cached_data = await self._redis.get(key)
            if cached_data:
                logger.debug("Order %s retrieved from cache", order_id)
                order_dict = json.loads(cached_data)
                order = PatientOrder(
                    id=order_dict["id"],
                    message=order_dict["message"],
                    patient_id=order_dict["patient_id"],
                    created_at=datetime.fromisoformat(order_dict["created_at"]),
                    updated_at=datetime.fromisoformat(order_dict["updated_at"]),
                )
                return order
        except Exception as e:
            logger.warning("Failed to get order from cache: %s", str(e))

        order = await self._inner_repository.get_order(order_id)
        if order is not None:
            await self._save_to_cache(order)
        return order

    async def add(self, order: PatientOrder) -> PatientOrder:
        """Add new order (invalidates patient cache)."""
        created_order = await self._inner_repository.add(order)
        await self._remove_from_cache(0, created_order.patient_id)
        return created_order

    async def update_message(self, order: PatientOrder) -> PatientOrder:
        """Update order message (invalidates order and patient cache)."""
        updated_order = await self._inner_repository.update_message(order)
        await self._remove_from_cache(order.id, updated_order.patient_id)
        return updated_order

    async def update(self, order_id: int, message: str, updated_at: datetime) -> Optional[PatientOrder]:
        """Update order with new message and timestamp (invalidates order and patient cache)."""
        updated_order = await self._inner_repository.update(order_id, message, updated_at)

        if updated_order is None:
            return None

        await self._remove_from_cache(order_id, updated_order.patient_id)
        return updated_order
