namespace Example.Api.Enums;

/// <summary>
/// The enum representing patient status.
/// </summary>
public enum PatientStatus : byte
{
    /// <summary>
    /// The active status.
    /// </summary>
    Active = 1,

    /// <summary>
    /// The inactive status.
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// The deceased status.
    /// </summary>
    Deceased = 3,

    /// <summary>
    /// The transferred status.
    /// </summary>
    Transferred = 4,

    /// <summary>
    /// The archived status.
    /// </summary>
    Archived = 5,

    /// <summary>
    /// The blacklisted status.
    /// </summary>
    Blacklisted = 6,
}
