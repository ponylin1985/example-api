"""Order entity model."""

from __future__ import annotations
from datetime import datetime
from typing import List, Optional, TYPE_CHECKING

from app.entities.base import Base
from app.entities.base_entity import BaseEntity
from sqlalchemy import DateTime, ForeignKey, String
from sqlalchemy.orm import Mapped, mapped_column, relationship
from app.enums.order import OrderType, OrderStatus
from app.db.types import IntEnumType

if TYPE_CHECKING:
    from app.entities.patient import Patient
    from app.entities.prescription import Prescription


class PatientOrder(Base, BaseEntity):
    """Order model."""

    __tablename__ = "patient_order"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    instructions: Mapped[Optional[str]] = mapped_column(String(500), nullable=True)
    next_visit_date: Mapped[Optional[datetime]] = mapped_column(DateTime(timezone=True), nullable=True)
    start_date: Mapped[Optional[datetime]] = mapped_column(DateTime(timezone=True), nullable=True)
    end_date: Mapped[Optional[datetime]] = mapped_column(DateTime(timezone=True), nullable=True)
    dispensed_date: Mapped[Optional[datetime]] = mapped_column(DateTime(timezone=True), nullable=True)
    type: Mapped[OrderType] = mapped_column(IntEnumType(OrderType), nullable=False, default=OrderType.Prescription)
    status: Mapped[OrderStatus] = mapped_column(IntEnumType(OrderStatus), nullable=False, default=OrderStatus.Created)

    patient_id: Mapped[int] = mapped_column(ForeignKey("patient.id", ondelete="CASCADE"), nullable=False)

    # Relationships
    patient: Mapped["Patient"] = relationship("Patient", back_populates="orders")
    prescriptions: Mapped[List["Prescription"]] = relationship(
        "Prescription", back_populates="order", cascade="all, delete-orphan"
    )
