"""API response codes enum."""

from enum import IntEnum


class ApiCode(IntEnum):
    """API response codes matching C# enum."""

    SUCCESS = 1
    UNKNOWN_ERROR = 2
    INVALID_REQUEST = 3
    NO_DATA_FOUND = 4
    DATA_ACCESS_ERROR = 5
    OPERATION_FAILED = 6
    OPERATION_TIMEOUT = 7
