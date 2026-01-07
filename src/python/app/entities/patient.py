"""Patient entity model."""

from __future__ import annotations
from app.entities.base import Base
from app.entities.base_entity import BaseEntity
from sqlalchemy import String
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import List, TYPE_CHECKING

if TYPE_CHECKING:
    from app.entities.order import Order


class Patient(Base, BaseEntity):
    """Patient model."""

    __tablename__ = "patient"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    name: Mapped[str] = mapped_column(String(50), nullable=False)

    # Relationship
    orders: Mapped[List["Order"]] = relationship("Order", back_populates="patient", cascade="all, delete-orphan")
