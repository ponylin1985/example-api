"""Order service."""

from sqlalchemy.ext.asyncio import AsyncSession

from app.models import Order
from app.repositories import OrderRepository, PatientRepository
from app.schemas import OrderDto, CreateOrderRequest, ApiResultData, ApiCode


class OrderService:
    """Service for order business logic."""

    def __init__(self, db: AsyncSession):
        """Initialize service with database session."""
        self.db = db
        self.order_repo = OrderRepository(db)
        self.patient_repo = PatientRepository(db)

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
            # Check if patient exists
            patient_exists = await self.patient_repo.is_exist_patient(request.patient_id)
            if not patient_exists:
                return ApiResultData[OrderDto](
                    success=False,
                    code=ApiCode.INVALID_REQUEST,
                    message=f"Patient with ID {request.patient_id} not found",
                    data=None,
                )

            # Create new order
            order = Order(message=request.message, patient_id=request.patient_id)

            created_order = await self.order_repo.add(order)
            await self.db.commit()
            await self.db.refresh(created_order)

            order_dto = OrderDto.model_validate(created_order)

            return ApiResultData[OrderDto](
                success=True, code=ApiCode.SUCCESS, message="Order created successfully", data=order_dto
            )

        except Exception as e:
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
            order = await self.order_repo.get_order(order_id)

            if not order:
                return ApiResultData[OrderDto](
                    success=False, code=ApiCode.NO_DATA_FOUND, message=f"Order with ID {order_id} not found", data=None
                )

            # Update message
            order.message = message
            updated_order = await self.order_repo.update(order)
            await self.db.commit()
            await self.db.refresh(updated_order)

            order_dto = OrderDto.model_validate(updated_order)

            return ApiResultData[OrderDto](
                success=True, code=ApiCode.SUCCESS, message="Order message updated successfully", data=order_dto
            )

        except Exception as e:
            await self.db.rollback()
            return ApiResultData[OrderDto](
                success=False,
                code=ApiCode.OPERATION_FAILED,
                message=f"Failed to update order message: {str(e)}",
                data=None,
            )
