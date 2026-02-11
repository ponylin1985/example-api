from enum import IntEnum


# Mirrors C# Example.Api.Enums.MedicationRoute
class MedicationRoute(IntEnum):
    Oral = 1
    Intravenous = 2
    Intramuscular = 3
    Subcutaneous = 4
    Intradermal = 5
    Inhalation = 6
    Topical = 7
    Sublingual = 8
    Rectal = 9
    Vaginal = 10
    Ophthalmic = 11
    Otic = 12
    Nasal = 13
    Other = 99
