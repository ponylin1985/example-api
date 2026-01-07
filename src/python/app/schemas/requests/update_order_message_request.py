"""Update order message request schema."""

from pydantic import BaseModel, Field, field_validator
from app.validators import SanitizerValidator


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

    @field_validator("message")
    @classmethod
    def validate_message_xss(cls, v: str) -> str:
        """Validate message for XSS attacks."""
        return SanitizerValidator.validate(v, "Message")
