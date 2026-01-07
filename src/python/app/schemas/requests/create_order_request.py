"""Create order request schema."""

from pydantic import BaseModel, Field, field_validator


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
