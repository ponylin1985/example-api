namespace Example.Api.Enums;

/// <summary>
/// The enum representing medication administration routes.
/// </summary>
public enum MedicationRoute : byte
{
    /// <summary>
    /// Oral administration. 口服
    /// </summary>
    Oral = 1,

    /// <summary>
    /// Intravenous injection. 靜脈注射
    /// </summary>
    Intravenous = 2,

    /// <summary>
    /// Intramuscular injection. 肌肉注射
    /// </summary>
    Intramuscular = 3,

    /// <summary>
    /// Subcutaneous injection. 皮下注射
    /// </summary>
    Subcutaneous = 4,

    /// <summary>
    /// Intradermal injection. 皮內注射
    /// </summary>
    Intradermal = 5,

    /// <summary>
    /// Inhalation. 吸入
    /// </summary>
    Inhalation = 6,

    /// <summary>
    /// Topical/skin application. 外用/皮膚
    /// </summary>
    Topical = 7,

    /// <summary>
    /// Sublingual administration. 舌下
    /// </summary>
    Sublingual = 8,

    /// <summary>
    /// Rectal (suppository) administration. 直腸（栓劑）
    /// </summary>
    Rectal = 9,

    /// <summary>
    /// Vaginal administration. 陰道
    /// </summary>
    Vaginal = 10,

    /// <summary>
    /// Ophthalmic (eye drops) administration. 點眼
    /// </summary>
    Ophthalmic = 11,

    /// <summary>
    /// Otic (ear drops) administration. 點耳
    /// </summary>
    Otic = 12,

    /// <summary>
    /// Nasal (nose drops) administration. 滴鼻
    /// </summary>
    Nasal = 13,

    /// <summary>
    /// Other routes. 其他
    /// </summary>
    Other = 99,
}
