from enum import IntEnum


# Mirrors C# Example.Api.Enums.LogType
class LogType(IntEnum):
    Unknown = 0
    Add = 1
    Update = 2
    Delete = 3
