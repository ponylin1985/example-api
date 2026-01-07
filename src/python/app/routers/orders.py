"""Order API endpoints."""

import logging
import redis.asyncio as redis
from fastapi import APIRouter, Body, Depends, Path
from sqlalchemy.ext.asyncio import AsyncSession
from app.infrastructure.database import get_db
from app.infrastructure.redis_client import get_redis
from app.services import OrderService
from app.schemas import CreateOrderRequest, UpdateOrderMessageRequest, OrderDto, ApiResultData

router = APIRouter(prefix="/api/orders", tags=["Orders"])
logger = logging.getLogger(__name__)


@router.get("/{order_id}", response_model=ApiResultData[OrderDto])
async def get_order_by_id(
    order_id: int = Path(..., gt=0, description="Order ID"),
    db: AsyncSession = Depends(get_db),
    redis_client: redis.Redis = Depends(get_redis),
):
    """
    Get an order by its identifier.
    """
    logger.info("Received request to get order with ID: %s", order_id)

    service = OrderService(db, redis_client)
    result = await service.get_order(order_id)

    if not result.success:
        logger.warning("Failed to retrieve order: %s", result.message)
    else:
        logger.info("Successfully retrieved order with ID: %s", result.data.id if result.data else "N/A")

    return result


@router.post("/", response_model=ApiResultData[OrderDto])
async def create_order(
    request: CreateOrderRequest,
    db: AsyncSession = Depends(get_db),
    redis_client: redis.Redis = Depends(get_redis),
):
    """
    Create a new order.
    """
    logger.info("Received request to create a new order for Patient ID: %s", request.patient_id)

    service = OrderService(db, redis_client)
    result = await service.create_order(request)

    if not result.success:
        logger.warning("Failed to create order: %s", result.message)
    else:
        logger.info("Successfully created order with ID: %s", result.data.id if result.data else "N/A")

    return result


@router.put("/{order_id}", response_model=ApiResultData[OrderDto])
async def update_order_message(
    order_id: int = Path(..., gt=0, description="Order ID"),
    request: UpdateOrderMessageRequest = Body(...),
    db: AsyncSession = Depends(get_db),
    redis_client: redis.Redis = Depends(get_redis),
):
    """
    Update the message of an existing order.
    """
    logger.info("Received request to update message for order with ID: %s", order_id)

    service = OrderService(db, redis_client)
    result = await service.update_message(order_id, request.message)

    if not result.success:
        logger.warning("Failed to update order message: %s", result.message)
    else:
        logger.info("Successfully updated message for order with ID: %s", result.data.id if result.data else "N/A")

    return result
