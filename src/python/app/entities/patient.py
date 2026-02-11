"""Patient entity model."""

from __future__ import annotations
from typing import List, TYPE_CHECKING, Optional
from datetime import date, datetime

from app.entities.base import Base
from app.entities.base_entity import BaseEntity
from app.enums.patient import Gender, PatientStatus
from app.db.types import IntEnumType
from sqlalchemy import String, Integer, Date, DateTime, Index, text, BigInteger
from sqlalchemy.orm import Mapped, mapped_column, relationship

if TYPE_CHECKING:
    from app.entities.patient_order import PatientOrder


class Patient(Base, BaseEntity):
    """Patient model."""

    __tablename__ = "patient"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True, autoincrement=True)
    name: Mapped[str] = mapped_column(String(50), nullable=False)
    age: Mapped[int] = mapped_column(Integer, nullable=False)
    gender: Mapped[Gender] = mapped_column(IntEnumType(Gender), nullable=False)
    email: Mapped[Optional[str]] = mapped_column(String(100), nullable=True)
    phone_number: Mapped[str] = mapped_column(String(10), nullable=False)

    # Address owned fields (flattened) matching EF Core names
    country: Mapped[Optional[str]] = mapped_column(String(25), nullable=True)
    city: Mapped[Optional[str]] = mapped_column(String(25), nullable=True)
    area: Mapped[Optional[str]] = mapped_column(String(25), nullable=True)
    road: Mapped[Optional[str]] = mapped_column(String(25), nullable=True)
    street: Mapped[Optional[str]] = mapped_column(String(25), nullable=True)
    address_others: Mapped[Optional[str]] = mapped_column(String(100), nullable=True)

    date_of_birth: Mapped[date] = mapped_column(Date, nullable=False)
    first_visit_date: Mapped[datetime] = mapped_column(DateTime(timezone=True), nullable=False)
    status: Mapped[PatientStatus] = mapped_column(
        IntEnumType(PatientStatus), nullable=False, default=PatientStatus.Active
    )
    remarks: Mapped[Optional[str]] = mapped_column(String(500), nullable=True)

    # Relationship
    orders: Mapped[List["PatientOrder"]] = relationship(
        "PatientOrder", back_populates="patient", cascade="all, delete-orphan", order_by="desc(PatientOrder.id)"
    )
