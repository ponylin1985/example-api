"""Order repository."""

from typing import Optional
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.models import Order


class OrderRepository:
    """Repository for order data access."""

    def __init__(self, db: AsyncSession):
        """Initialize repository with database session."""
        self.db = db

    async def get_order(self, order_id: int) -> Optional[Order]:
        """Get order by ID."""
        query = select(Order).where(Order.id == order_id)
        result = await self.db.execute(query)
        return result.scalar_one_or_none()

    async def add(self, order: Order) -> Order:
        """Add new order."""
        self.db.add(order)
        await self.db.flush()
        await self.db.refresh(order)
        return order

    async def update(self, order: Order) -> Order:
        """Update order."""
        await self.db.flush()
        await self.db.refresh(order)
        return order
