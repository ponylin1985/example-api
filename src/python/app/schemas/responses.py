"""API response enums and models."""

from enum import IntEnum
from typing import Optional, TypeVar, Generic, List
from pydantic import BaseModel


class ApiCode(IntEnum):
    """API response codes matching C# enum."""

    SUCCESS = 1
    UNKNOWN_ERROR = 2
    INVALID_REQUEST = 3
    NO_DATA_FOUND = 4
    DATA_ACCESS_ERROR = 5
    OPERATION_FAILED = 6
    OPERATION_TIMEOUT = 7


class ApiResult(BaseModel):
    """Base API result matching C# ApiResult."""

    success: bool
    code: ApiCode
    message: Optional[str] = ""


T = TypeVar("T")


class ApiResultData(ApiResult, Generic[T]):
    """API result with data matching C# ApiResult<T>."""

    data: Optional[T] = None


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
