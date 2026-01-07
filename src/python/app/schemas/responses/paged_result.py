"""Paged result schema."""

from typing import TypeVar, Generic, List
from pydantic import BaseModel


T = TypeVar("T")


class PagedResult(BaseModel, Generic[T]):
    """Paged result matching C# PagedResult<T>."""

    data: List[T]
    page_number: int
    total_pages: int
    total_count: int
    page_size: int

    @property
    def has_previous_page(self) -> bool:
        """Check if there is a previous page."""
        return self.page_number > 1

    @property
    def has_next_page(self) -> bool:
        """Check if there is a next page."""
        return self.page_number < self.total_pages

    class Config:
        """Pydantic configuration."""

        populate_by_name = True
        json_schema_extra = {
            "example": {"data": [], "page_number": 1, "total_pages": 1, "total_count": 0, "page_size": 10}
        }
