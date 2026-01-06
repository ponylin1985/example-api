"""Database models."""

from __future__ import annotations
from datetime import datetime
from typing import Optional, List
from sqlalchemy import String, Text, ForeignKey, DateTime
from sqlalchemy.orm import DeclarativeBase, Mapped, mapped_column, relationship


class Base(DeclarativeBase):
    """Base class for all models."""


class BaseEntity:
    """Base entity with created_at and updated_at timestamps."""

    created_at: Mapped[datetime] = mapped_column(DateTime(timezone=True), nullable=False, default=datetime.utcnow)
    updated_at: Mapped[Optional[datetime]] = mapped_column(
        DateTime(timezone=True), nullable=True, default=None, onupdate=datetime.utcnow
    )


class Patient(Base, BaseEntity):
    """Patient model."""

    __tablename__ = "patients"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    name: Mapped[str] = mapped_column(String(50), nullable=False)

    # Relationship
    orders: Mapped[List["Order"]] = relationship("Order", back_populates="patient", cascade="all, delete-orphan")


class Order(Base, BaseEntity):
    """Order model."""

    __tablename__ = "orders"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    message: Mapped[str] = mapped_column(Text, nullable=False)
    patient_id: Mapped[int] = mapped_column(ForeignKey("patients.id", ondelete="CASCADE"), nullable=False)

    # Relationship
    patient: Mapped["Patient"] = relationship("Patient", back_populates="orders")
