"""Patient repository."""

from __future__ import annotations
from app.entities import Patient, Order
from datetime import datetime
from sqlalchemy import func as sql_func
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.orm import selectinload
from typing import Optional, Tuple, List


class PatientRepository:
    """Repository for patient data access."""

    def __init__(self, db: AsyncSession):
        """Initialize repository with database session."""
        self.db = db

    async def get_patients(
        self, start_time: datetime, end_time: datetime, page_number: int, page_size: int
    ) -> Tuple[List[Patient], int]:
        """
        Get patients within date range with pagination.

        Args:
            start_time: Start of date range
            end_time: End of date range
            page_number: Page number (1-indexed)
            page_size: Number of items per page

        Returns:
            Tuple of (list of patients, total count)
        """
        # pylint: disable=not-callable
        count_query = (
            select(sql_func.count(Patient.id))
            .select_from(Patient)
            .where(Patient.created_at >= start_time, Patient.created_at <= end_time)
        )
        total_count_result = await self.db.execute(count_query)
        total_count = total_count_result.scalar() or 0

        offset = (page_number - 1) * page_size
        data_query = (
            select(Patient)
            .options(selectinload(Patient.orders))
            .where(Patient.created_at >= start_time, Patient.created_at <= end_time)
            .order_by(Patient.created_at.desc())
            .offset(offset)
            .limit(page_size)
        )

        result = await self.db.execute(data_query)
        patients = list(result.scalars().all())

        return patients, total_count

    async def is_exist_patient(self, patient_id: int) -> bool:
        """Check if patient exists by ID."""
        # pylint: disable=not-callable
        query = select(sql_func.count()).select_from(Patient).where(Patient.id == patient_id)
        result = await self.db.execute(query)
        count = result.scalar() or 0
        return count > 0

    async def get_patient(self, patient_id: int) -> Optional[Patient]:
        """Get patient by ID with orders."""
        query = select(Patient).options(selectinload(Patient.orders)).where(Patient.id == patient_id)
        result = await self.db.execute(query)
        return result.scalar_one_or_none()

    async def get_patient_by_name(self, name: str) -> Optional[Patient]:
        """Get patient by name."""
        query = select(Patient).where(Patient.name == name)
        result = await self.db.execute(query)
        return result.scalar_one_or_none()

    async def add(self, patient: Patient) -> Patient:
        """Add new patient."""
        self.db.add(patient)
        await self.db.flush()
        await self.db.refresh(patient)
        return patient
