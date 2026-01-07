"""Patient DTO schema."""

from datetime import datetime
from typing import Optional, List
from pydantic import BaseModel, ConfigDict
from app.schemas.dtos.order_dto import OrderDto


class PatientDto(BaseModel):
    """Patient DTO matching C# PatientDto."""

    id: int
    name: str
    created_at: datetime
    updated_at: Optional[datetime] = None
    orders: List[OrderDto] = []

    model_config = ConfigDict(
        from_attributes=True,
        populate_by_name=True,
        json_schema_extra={
            "example": {
                "id": 1,
                "name": "John Doe",
                "created_at": "2025-12-15T10:00:00Z",
                "updated_at": "2025-12-15T10:00:00Z",
                "orders": [],
            }
        },
    )
