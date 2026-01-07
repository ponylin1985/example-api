"""Cached patient repository with Redis decorator."""

import json
import logging
from app.configs.cache_config import cache_settings
from app.entities import Order
from app.entities import Patient
from app.repositories.patient_repository import PatientRepository
from datetime import datetime
from typing import Optional, Tuple, List
import redis.asyncio as redis

logger = logging.getLogger(__name__)


class CachedPatientRepository:
    """Decorator for PatientRepository that adds Redis caching."""

    def __init__(self, inner_repository: PatientRepository, redis_client: redis.Redis):
        """
        Initialize cached repository.

        Args:
            inner_repository: The inner patient repository to which calls are delegated
            redis_client: Redis client for caching
        """
        self._inner_repository = inner_repository
        self._redis = redis_client
        self._ttl = cache_settings.cache_ttl_seconds

    @staticmethod
    def _get_existence_cache_key(patient_id: int) -> str:
        """Get cache key for a patient."""
        return f"patient:{patient_id}"

    async def _save_to_cache(self, patient: Patient) -> None:
        """Save patient to cache."""
        try:
            key = self._get_existence_cache_key(patient.id)
            patient_dict = {
                "id": patient.id,
                "name": patient.name,
                "created_at": patient.created_at.isoformat(),
                "updated_at": patient.updated_at.isoformat(),
                "orders": [
                    {
                        "id": order.id,
                        "message": order.message,
                        "patient_id": order.patient_id,
                        "created_at": order.created_at.isoformat(),
                        "updated_at": order.updated_at.isoformat(),
                    }
                    for order in patient.orders
                ],
            }
            await self._redis.setex(key, self._ttl, json.dumps(patient_dict))
            logger.debug("Saved patient %s to cache", patient.id)
        except Exception as e:
            logger.error("Error occurred while saving patient to cache with key %s: %s", key, str(e))

    async def get_patient(self, patient_id: int) -> Optional[Patient]:
        """Get patient by ID (with caching)."""
        key = self._get_existence_cache_key(patient_id)

        try:
            cached_data = await self._redis.get(key)
            if cached_data:
                logger.debug("Patient %s retrieved from cache", patient_id)
                patient_dict = json.loads(cached_data)
                patient = Patient(
                    id=patient_dict["id"],
                    name=patient_dict["name"],
                    created_at=datetime.fromisoformat(patient_dict["created_at"]),
                    updated_at=datetime.fromisoformat(patient_dict["updated_at"]),
                )

                patient.orders = [
                    Order(
                        id=order_dict["id"],
                        message=order_dict["message"],
                        patient_id=order_dict["patient_id"],
                        created_at=datetime.fromisoformat(order_dict["created_at"]),
                        updated_at=datetime.fromisoformat(order_dict["updated_at"]),
                    )
                    for order_dict in patient_dict.get("orders", [])
                ]

                return patient
        except Exception as e:
            logger.warning("Failed to get patient from cache: %s", str(e))

        patient = await self._inner_repository.get_patient(patient_id)
        if patient is not None:
            await self._save_to_cache(patient)
        return patient

    async def is_exist_patient(self, patient_id: int) -> bool:
        """Check if patient exists (with caching)."""
        key = self._get_existence_cache_key(patient_id)

        try:
            if await self._redis.exists(key):
                logger.debug("Patient existence check from cache: %s", patient_id)
                return True
        except Exception as e:
            logger.warning("Failed to check patient existence from cache: %s", str(e))

        exists = await self._inner_repository.is_exist_patient(patient_id)
        return exists

    async def get_patients(
        self, start_time: datetime, end_time: datetime, page_number: int, page_size: int
    ) -> Tuple[List[Patient], int]:
        """
        Get patients within date range with pagination.
        Note: List queries are not cached due to complexity.
        """
        return await self._inner_repository.get_patients(start_time, end_time, page_number, page_size)

    async def get_patient_by_name(self, name: str) -> Optional[Patient]:
        """Get patient by name (no caching for this query)."""
        return await self._inner_repository.get_patient_by_name(name)

    async def add(self, patient: Patient) -> Patient:
        """Add new patient (no cache invalidation needed for new records)."""
        return await self._inner_repository.add(patient)
