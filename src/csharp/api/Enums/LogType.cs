namespace Example.Api.Enums;

/// <summary>
/// Defines the types of logs that can be recorded.
/// </summary>
public enum LogType : byte
{
    Unknown = 0,
    Add = 1,
    Update = 2,
    Delete = 3,
}
