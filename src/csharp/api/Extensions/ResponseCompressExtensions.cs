using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Example.Api.Extensions;

/// <summary>
/// Extension methods for adding response compression services.
/// </summary>
public static class ResponseCompressExtensions
{
    /// <summary>
    /// Adds response compression services to the IServiceCollection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();
        });

        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        return services;
    }
}
