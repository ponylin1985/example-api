"""Create patient request schema."""

from pydantic import BaseModel, Field, field_validator


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
