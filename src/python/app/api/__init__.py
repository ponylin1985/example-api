"""API endpoints package."""

from app.api import patient_endpoints, order_endpoints

__all__ = [
    "patient_endpoints",
    "order_endpoints",
]
