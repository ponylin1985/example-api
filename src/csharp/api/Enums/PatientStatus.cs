namespace Example.Api.Enums;

/// <summary>
/// The enum representing patient status.
/// </summary>
public enum PatientStatus : byte
{
    /// <summary>
    /// The active status. 活動中/在籍
    /// </summary>
    Active = 1,

    /// <summary>
    /// The inactive status. 非活動/停用
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// The deceased status. 已故
    /// </summary>
    Deceased = 3,

    /// <summary>
    /// The transferred status. 轉院/轉出
    /// </summary>
    Transferred = 4,

    /// <summary>
    /// The archived status. 歸檔/封存
    /// </summary>
    Archived = 5,

    /// <summary>
    /// The blacklisted status. 黑名單
    /// </summary>
    Blacklisted = 6,
}
