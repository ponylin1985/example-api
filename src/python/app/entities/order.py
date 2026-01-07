"""Order entity model."""

from __future__ import annotations
from app.entities.base import Base
from app.entities.base_entity import BaseEntity
from sqlalchemy import Text, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from typing import List, TYPE_CHECKING

if TYPE_CHECKING:
    from app.entities.patient import Patient


class Order(Base, BaseEntity):
    """Order model."""

    __tablename__ = "order"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    message: Mapped[str] = mapped_column(Text, nullable=False)
    patient_id: Mapped[int] = mapped_column(ForeignKey("patient.id", ondelete="CASCADE"), nullable=False)

    # Relationship
    patient: Mapped["Patient"] = relationship("Patient", back_populates="orders")
