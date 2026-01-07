"""Get patients request schema."""

from datetime import datetime
from pydantic import Field, field_validator
from app.schemas.requests.paged_request import PagedRequest


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
