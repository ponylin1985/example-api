namespace Example.Api.Enums;

/// <summary>
/// The enum representing order status.
/// </summary>
public enum OrderStatus : byte
{
    /// <summary>
    /// Created. 已建立
    /// </summary>
    Created,

    /// <summary>
    /// Dispensed. 已發藥/已配藥
    /// </summary>
    Dispensed,

    /// <summary>
    /// Executed. 已執行
    /// </summary>
    Executed,

    /// <summary>
    /// Cancelled. 已取消
    /// </summary>
    Cancelled,

    /// <summary>
    /// Expired. 已過期
    /// </summary>
    Expired,
}
