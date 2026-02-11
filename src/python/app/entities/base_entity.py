"""Base entity with common audit fields matching C# BaseEntity."""

from __future__ import annotations
from datetime import datetime
from typing import Optional
from sqlalchemy import DateTime, String, text
from sqlalchemy.orm import Mapped, mapped_column


class BaseEntity:
    """Base entity with created_by/created_at and updated_by/updated_at.

    Column names and defaults mirror the EF Core configuration:
    - `created_by` / `updated_by`: varchar(50) NOT NULL
    - `created_at` / `updated_at`: timestamptz with server default TIMEZONE('utc', NOW())
    """

    created_by: Mapped[str] = mapped_column(String(50), nullable=False, default="")
    created_at: Mapped[datetime] = mapped_column(
        DateTime(timezone=True), nullable=False, server_default=text("TIMEZONE('utc', NOW())")
    )
    updated_by: Mapped[str] = mapped_column(String(50), nullable=False, default="")
    updated_at: Mapped[datetime] = mapped_column(
        DateTime(timezone=True), nullable=False, server_default=text("TIMEZONE('utc', NOW())")
    )
