"""Order DTO schema."""

from datetime import datetime
from typing import Optional
from pydantic import BaseModel, ConfigDict


class OrderDto(BaseModel):
    """Order DTO matching C# OrderDto."""

    id: int
    message: str
    patient_id: int
    created_at: datetime
    updated_at: Optional[datetime] = None

    model_config = ConfigDict(
        from_attributes=True,
        populate_by_name=True,
        json_schema_extra={
            "example": {
                "id": 1,
                "message": "Lab results review",
                "patient_id": 1,
                "created_at": "2025-12-15T10:30:00Z",
                "updated_at": "2025-12-15T10:30:00Z",
            }
        },
    )
