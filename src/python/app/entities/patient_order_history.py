"""PatientOrderHistory entity model."""

from __future__ import annotations
from datetime import datetime
from app.entities.base import Base
from app.enums.log_type import LogType
from app.enums.order import OrderStatus
from app.db.types import IntEnumType
from sqlalchemy import DateTime, ForeignKey, Index, String
from sqlalchemy import Integer
from sqlalchemy.orm import Mapped, mapped_column


class PatientOrderHistory(Base):
    """Patient order history model."""

    __tablename__ = "patient_order_history"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement=True)
    type: Mapped[LogType] = mapped_column(IntEnumType(LogType), nullable=False, default=LogType.Add)
    order_id: Mapped[int] = mapped_column(ForeignKey("patient_order.id", ondelete="RESTRICT"), nullable=False)
    patient_id: Mapped[int] = mapped_column(Integer, nullable=False)
    status: Mapped[OrderStatus] = mapped_column(IntEnumType(OrderStatus), nullable=False)
    remarks: Mapped[str] = mapped_column(String(255), nullable=True)
    log_by: Mapped[str] = mapped_column(String(50), nullable=False)
    log_at: Mapped[datetime] = mapped_column(DateTime(timezone=True), nullable=False)
