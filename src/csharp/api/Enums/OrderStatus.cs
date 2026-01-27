namespace Example.Api.Enums;

/// <summary>
/// The enum representing order status.
/// </summary>
public enum OrderStatus : byte
{
    /// <summary>
    /// Created. 已建立
    /// </summary>
    Created = 1,

    /// <summary>
    /// Dispensed. 已發藥/已配藥
    /// </summary>
    Dispensed = 2,

    /// <summary>
    /// Executed. 已執行
    /// </summary>
    Executed = 3,

    /// <summary>
    /// Cancelled. 已取消
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Expired. 已過期
    /// </summary>
    Expired = 5,
}
