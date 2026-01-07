"""Base API result schema."""

from typing import Optional
from pydantic import BaseModel
from app.schemas.responses.api_code import ApiCode


class ApiResult(BaseModel):
    """Base API result matching C# ApiResult."""

    success: bool
    code: ApiCode
    message: Optional[str] = ""
