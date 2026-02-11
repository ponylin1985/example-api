"""Prescription entity model."""

from __future__ import annotations
from typing import Optional
from app.db.types import IntEnumType
from app.entities.base import Base
from app.entities.base_entity import BaseEntity
from app.entities.medication import Medication
from app.enums.medication_route import MedicationRoute
from sqlalchemy import ForeignKey, Integer, String, Index, BigInteger
from sqlalchemy.orm import Mapped, mapped_column, relationship


class Prescription(Base, BaseEntity):
    """Prescription model."""

    __tablename__ = "prescription"

    id: Mapped[int] = mapped_column(BigInteger, primary_key=True, autoincrement=True)
    order_id: Mapped[int] = mapped_column(
        BigInteger, ForeignKey("patient_order.id", ondelete="RESTRICT"), nullable=False
    )
    medication_id: Mapped[int] = mapped_column(
        BigInteger, ForeignKey("medication.id", ondelete="RESTRICT"), nullable=False
    )

    dose: Mapped[Optional[str]] = mapped_column(String(50), nullable=True)
    frequency: Mapped[Optional[str]] = mapped_column(String(50), nullable=True)
    duration_in_days: Mapped[int] = mapped_column(Integer, nullable=False)

    route: Mapped[MedicationRoute] = mapped_column(IntEnumType(MedicationRoute), nullable=False)

    # Relationships
    # Prescription has medication navigation (many -> one). C# Prescription does not expose an Order navigation.
    medication: Mapped["Medication"] = relationship("Medication", back_populates="prescriptions")


# Indexes to match EF Core configuration
Index("ix_prescription_order_id", "order_id")
Index("ix_prescription_medication_id", "medication_id")
