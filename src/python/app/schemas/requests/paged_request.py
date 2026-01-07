"""Paged request schema."""

from pydantic import BaseModel, Field


class PagedRequest(BaseModel):
    """Base paged request matching C# PagedRequest."""

    page_number: int = Field(default=1, ge=1, description="Page number to retrieve")
    page_size: int = Field(default=10, ge=1, le=100, description="Number of items per page")
