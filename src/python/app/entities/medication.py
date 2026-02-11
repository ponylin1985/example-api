"""Medication entity model."""

from __future__ import annotations
from app.entities.base import Base
from app.entities.base_entity import BaseEntity
from sqlalchemy import Boolean, String
from sqlalchemy.orm import Mapped, mapped_column


class Medication(Base, BaseEntity):
    """Medication model."""

    __tablename__ = "medication"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    name: Mapped[str] = mapped_column(String(50), nullable=False)
    manufacturer: Mapped[str] = mapped_column(String(50), nullable=False)
    is_enabled: Mapped[bool] = mapped_column(Boolean, nullable=False, default=True)
