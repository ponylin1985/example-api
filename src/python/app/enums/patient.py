from enum import IntEnum


# Mirrors C# Example.Api.Enums.PatientStatus
class PatientStatus(IntEnum):
    Active = 1
    Inactive = 2
    Deceased = 3
    Transferred = 4
    Archived = 5
    Blacklisted = 6


# Mirrors C# Example.Api.Enums.Gender
class Gender(IntEnum):
    Male = 1
    Female = 0
