namespace Example.Api.Options;

/// <summary>
/// Options for request and response logging.
/// </summary>
public class RequestResponseLoggingOptions
{
    /// <summary>
    /// Indicates whether request logging is enabled.
    /// </summary>
    /// <value></value>
    public bool EnabledRequestLog { get; set; }

    /// <summary>
    /// Indicates whether response logging is enabled.
    /// </summary>
    /// <value></value>
    public bool EnabledResponseLog { get; set; }
}
