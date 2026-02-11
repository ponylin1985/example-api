"""Order repository."""

from app.entities import PatientOrder
from sqlalchemy import select, update
from sqlalchemy.ext.asyncio import AsyncSession
from typing import Optional
from datetime import datetime


class OrderRepository:
    """Repository for order data access."""

    def __init__(self, db: AsyncSession):
        """Initialize repository with database session."""
        self.db = db

    async def get_order(self, order_id: int) -> Optional[PatientOrder]:
        """Get order by ID."""
        query = select(PatientOrder).where(PatientOrder.id == order_id)
        result = await self.db.execute(query)
        return result.scalar_one_or_none()

    async def add(self, order: PatientOrder) -> PatientOrder:
        """Add new order."""
        self.db.add(order)
        await self.db.flush()
        await self.db.refresh(order)
        return order

    async def update_message(self, order: PatientOrder) -> PatientOrder:
        """
        Update order message.
        Similar to C# UpdateMessageAsync - finds the order by ID and updates its message.

        Args:
            order: Order with ID and new message

        Returns:
            Updated order

        Raises:
            ValueError: If order not found
        """
        query = select(PatientOrder).where(PatientOrder.id == order.id)
        result = await self.db.execute(query)
        existing_order = result.scalar_one_or_none()

        if not existing_order:
            raise ValueError(f"OrderId {order.id} not found.")

        existing_order.message = order.message
        await self.db.flush()
        await self.db.refresh(existing_order)
        return existing_order

    async def update(self, order_id: int, message: str, updated_at: datetime) -> Optional[PatientOrder]:
        """
        Update order with new message and updated_at timestamp.
        Similar to C# UpdateAsync - uses UPDATE statement and returns new Order object.

        Args:
            order_id: Order ID
            message: New message
            updated_at: Updated timestamp

        Returns:
            Updated order or None if not found
        """
        query = select(PatientOrder.patient_id, PatientOrder.created_at).where(PatientOrder.id == order_id)
        result = await self.db.execute(query)
        row = result.first()

        if not row:
            return None

        patient_id, created_at = row

        stmt = update(PatientOrder).where(PatientOrder.id == order_id).values(message=message, updated_at=updated_at)
        await self.db.execute(stmt)
        await self.db.flush()

        return PatientOrder(
            id=order_id,
            message=message,
            patient_id=patient_id,
            created_at=created_at,
            updated_at=updated_at,
        )
