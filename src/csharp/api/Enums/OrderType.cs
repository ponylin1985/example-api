namespace Example.Api.Enums;

/// <summary>
/// The enum representing order types.
/// 訂單/醫囑類型的列舉型別。
/// </summary>
public enum OrderType : byte
{
    /// <summary>
    /// Prescription order. 處方醫囑
    /// </summary>
    Prescription,

    /// <summary>
    /// Laboratory order. 檢驗醫囑
    /// </summary>
    Lab,

    /// <summary>
    /// Imaging order. 影像醫囑
    /// </summary>
    Imaging,

    /// <summary>
    /// Treatment order. 治療醫囑
    /// </summary>
    Treatment,

    /// <summary>
    /// Surgery order. 手術醫囑
    /// </summary>
    Surgery,

    /// <summary>
    /// Other order types. 其他
    /// </summary>
    Other,
}
