from enum import IntEnum


# Mirrors C# Example.Api.Enums.OrderType (values start at 1)
class OrderType(IntEnum):
    Prescription = 1
    Lab = 2
    Imaging = 3
    Treatment = 4
    Surgery = 5
    Other = 6


# Mirrors C# Example.Api.Enums.OrderStatus
class OrderStatus(IntEnum):
    Created = 1
    Dispensed = 2
    Executed = 3
    Cancelled = 4
    Expired = 5
