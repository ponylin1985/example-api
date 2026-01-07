"""API result with data schema."""

from typing import TypeVar, Generic, Optional
from app.schemas.responses.api_result import ApiResult


T = TypeVar("T")


class ApiResultData(ApiResult, Generic[T]):
    """API result with data matching C# ApiResult<T>."""

    data: Optional[T] = None
