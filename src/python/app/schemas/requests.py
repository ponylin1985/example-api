"""Request schemas."""

from datetime import datetime
from pydantic import BaseModel, Field, field_validator


class PagedRequest(BaseModel):
    """Base paged request matching C# PagedRequest."""

    page_number: int = Field(default=1, ge=1, description="Page number to retrieve")
    page_size: int = Field(default=10, ge=1, le=100, description="Number of items per page")


class GetPatientsRequest(PagedRequest):
    """Request to get patients within a date range matching C# GetPatientsRequest."""

    start_time: datetime = Field(..., description="Start time of the date range (UTC)")
    end_time: datetime = Field(..., description="End time of the date range (UTC)")

    @field_validator("end_time")
    @classmethod
    def validate_end_time(cls, v: datetime, info) -> datetime:
        """Validate that end_time is after start_time."""
        if "start_time" in info.data and v < info.data["start_time"]:
            raise ValueError("EndTime must be greater than or equal to StartTime.")
        return v


class CreatePatientRequest(BaseModel):
    """Request to create a new patient matching C# CreatePatientRequest."""

    name: str = Field(..., max_length=50, description="Name of the patient")
    order_message: str = Field(..., max_length=500, description="Message for the patient's order")

    @field_validator("name", "order_message")
    @classmethod
    def validate_not_empty(cls, v: str) -> str:
        """Validate that string fields are not empty."""
        if not v or not v.strip():
            raise ValueError("Field cannot be empty")
        return v


class CreateOrderRequest(BaseModel):
    """Request to create a new order matching C# CreateOrderRequest."""

    patient_id: int = Field(..., gt=0, description="The id of the patient related to the order")
    message: str = Field(..., max_length=500, description="The message associated with the order")

    @field_validator("message")
    @classmethod
    def validate_message(cls, v: str) -> str:
        """Validate that message is not empty."""
        if not v or not v.strip():
            raise ValueError("Message cannot be empty")
        return v


class UpdateOrderMessageRequest(BaseModel):
    """Request to update an order's message matching C# UpdateOrderMessageRequest."""

    message: str = Field(..., max_length=500, description="Message to update the order with")

    @field_validator("message")
    @classmethod
    def validate_message(cls, v: str) -> str:
        """Validate that message is not empty."""
        if not v or not v.strip():
            raise ValueError("Message cannot be empty")
        return v
