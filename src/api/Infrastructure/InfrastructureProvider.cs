using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Example.Api.Infrastructure;

/// <summary>
/// Provides extension methods for registering infrastructure services.
/// </summary>
public static class InfrastructureProvider
{
    /// <summary>
    /// Registers infrastructure services with the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration to read settings from.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbSession, ApplicationDbSession>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("DefaultRedisConnection")!));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("DefaultRedisConnection");
        });
        return services;
    }

    /// <summary>
    /// Adds JSON serialization options to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJsonSerializationOptions(this IServiceCollection services)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
        };

        services.AddSingleton(jsonOptions);

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = jsonOptions.PropertyNamingPolicy;
            options.SerializerOptions.NumberHandling = jsonOptions.NumberHandling;
            options.SerializerOptions.DefaultIgnoreCondition = jsonOptions.DefaultIgnoreCondition;
            options.SerializerOptions.AllowTrailingCommas = jsonOptions.AllowTrailingCommas;
            options.SerializerOptions.PropertyNameCaseInsensitive = jsonOptions.PropertyNameCaseInsensitive;
            options.SerializerOptions.ReferenceHandler = jsonOptions.ReferenceHandler;
            foreach (var converter in jsonOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }
        });

        return services;
    }

    /// <summary>
    /// Adds default cache options to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCacheOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var strSlidingMinutes = configuration["CacheOptions:SlidingExpirationMinutes"];
        var strAbsoluteMinutes = configuration["CacheOptions:AbsoluteExpirationRelativeToNowMinutes"];

        var slidingMinutes = int.TryParse(strSlidingMinutes, out var s) ? s : 2;
        var absoluteMinutes = int.TryParse(strAbsoluteMinutes, out var a) ? a : 10;

        services.Configure<DistributedCacheEntryOptions>(options =>
        {
            options.SlidingExpiration = TimeSpan.FromMinutes(slidingMinutes);
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteMinutes);
        });

        return services;
    }
}
