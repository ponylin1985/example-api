"""Order service."""

import logging
import redis.asyncio as redis
from datetime import datetime
from app.entities import Order
from app.repositories import OrderRepository, PatientRepository
from app.repositories.caches.cached_order_repository import CachedOrderRepository
from app.repositories.caches.cached_patient_repository import CachedPatientRepository
from app.schemas import OrderDto, CreateOrderRequest, ApiResultData, ApiCode
from sqlalchemy.ext.asyncio import AsyncSession

logger = logging.getLogger(__name__)


class OrderService:
    """Service for order business logic."""

    def __init__(self, db: AsyncSession, redis_client: redis.Redis):
        """Initialize service with database session and Redis client."""
        self.db = db
        inner_order_repo = OrderRepository(db)
        inner_patient_repo = PatientRepository(db)
        self.order_repo = CachedOrderRepository(inner_order_repo, redis_client)
        self.patient_repo = CachedPatientRepository(inner_patient_repo, redis_client)

    async def get_order(self, order_id: int) -> ApiResultData[OrderDto]:
        """
        Get order by ID.

        Args:
            order_id: Order ID

        Returns:
            API result with order data
        """
        try:
            order = await self.order_repo.get_order(order_id)

            if not order:
                return ApiResultData[OrderDto](
                    success=False, code=ApiCode.NO_DATA_FOUND, message=f"Order with ID {order_id} not found", data=None
                )

            order_dto = OrderDto.model_validate(order)
            return ApiResultData[OrderDto](
                success=True, code=ApiCode.SUCCESS, message="Order retrieved successfully", data=order_dto
            )

        except Exception as e:
            logger.error("Failed to retrieve order: %s", str(e), exc_info=True)
            return ApiResultData[OrderDto](
                success=False, code=ApiCode.DATA_ACCESS_ERROR, message=f"Failed to retrieve order: {str(e)}", data=None
            )

    async def create_order(self, request: CreateOrderRequest) -> ApiResultData[OrderDto]:
        """
        Create a new order for a patient.

        Args:
            request: Request with order data

        Returns:
            API result with created order data
        """
        try:
            patient_exists = await self.patient_repo.is_exist_patient(request.patient_id)
            if not patient_exists:
                return ApiResultData[OrderDto](
                    success=False,
                    code=ApiCode.INVALID_REQUEST,
                    message=f"Patient with ID {request.patient_id} not found",
                    data=None,
                )

            order = Order(message=request.message, patient_id=request.patient_id)

            created_order = await self.order_repo.add(order)
            await self.db.commit()
            await self.db.refresh(created_order)

            order_dto = OrderDto.model_validate(created_order)
            return ApiResultData[OrderDto](
                success=True, code=ApiCode.SUCCESS, message="Order created successfully", data=order_dto
            )

        except Exception as e:
            logger.error("Failed to create order: %s", str(e), exc_info=True)
            await self.db.rollback()
            return ApiResultData[OrderDto](
                success=False, code=ApiCode.OPERATION_FAILED, message=f"Failed to create order: {str(e)}", data=None
            )

    async def update_message(self, order_id: int, message: str) -> ApiResultData[OrderDto]:
        """
        Update order message.

        Args:
            order_id: Order ID
            message: New message

        Returns:
            API result with updated order data
        """
        try:
            utc_now = datetime.utcnow()
            updated_order = await self.order_repo.update(order_id, message.strip(), utc_now)

            if not updated_order:
                return ApiResultData[OrderDto](
                    success=False,
                    code=ApiCode.OPERATION_FAILED,
                    message=f"Order with ID {order_id} not found.",
                    data=None,
                )

            await self.db.commit()

            order_dto = OrderDto.model_validate(updated_order)

            return ApiResultData[OrderDto](
                success=True, code=ApiCode.SUCCESS, message="Order message updated successfully", data=order_dto
            )

        except Exception as e:
            logger.error("Failed to update order message: %s", str(e), exc_info=True)
            await self.db.rollback()
            return ApiResultData[OrderDto](
                success=False,
                code=ApiCode.OPERATION_FAILED,
                message=f"Failed to update order message: {str(e)}",
                data=None,
            )
