using Example.Api.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using StackExchange.Redis;
using System.Text.Json;
using Order = Example.Api.Models.Order;

namespace Example.Api.Repositories.Caches;

/// <summary>
/// Decorator for IOrderRepository that adds caching.
/// </summary>
public class CachedOrderRepository : IOrderRepository
{
    /// <summary>
    /// Logger for the CachedOrderRepository.
    /// </summary>
    private readonly ILogger<CachedOrderRepository> _logger;

    /// <summary>
    /// The inner IOrderRepository instance being decorated.
    /// </summary>
    private readonly IOrderRepository _innerRepository;

    /// <summary>
    /// The distributed cache instance used for caching orders.
    /// </summary>
    private readonly IDistributedCache _cache;

    /// <summary>
    /// The Redis connection multiplexer.
    /// </summary>
    private readonly IConnectionMultiplexer _redisConnection;

    /// <summary>
    /// Options for cache entry expiration.
    /// </summary>
    private readonly IOptionsMonitor<DistributedCacheEntryOptions> _cacheOptions;

    /// <summary>
    /// Retry policy for cache operations.
    /// </summary>
    private readonly AsyncRetryPolicy _retryPolicy;

    /// <summary>
    /// JSON serializer options to handle reference cycles.
    /// </summary>
    /// <returns></returns>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedOrderRepository"/> class.
    /// </summary>
    /// <param name="logger">Logger for the CachedOrderRepository.</param>
    /// <param name="innerRepository">The inner IOrderRepository instance being decorated.</param>
    /// <param name="cache">The distributed cache instance used for caching orders.</param>
    /// <param name="cacheOptions">Cache entry options to configure cache expiration.</param>
    /// <param name="redisConnection">The Redis connection multiplexer.</param>
    /// <param name="jsonOptions">JSON serializer options to handle reference cycles.</param>
    public CachedOrderRepository(
        ILogger<CachedOrderRepository> logger,
        IOrderRepository innerRepository,
        IDistributedCache cache,
        IOptionsMonitor<DistributedCacheEntryOptions> cacheOptions,
        IConnectionMultiplexer redisConnection,
        JsonSerializerOptions jsonOptions)
    {
        _logger = logger;
        _innerRepository = innerRepository;
        _cache = cache;
        _cacheOptions = cacheOptions;
        _redisConnection = redisConnection;
        _jsonOptions = jsonOptions;

        var jitterer = new Random();
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                    TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Cache write failed. Retrying in {TimeSpan}. Attempt {RetryCount}.",
                        timeSpan,
                        retryCount);
                });
    }

    /// <inheritdoc />
    public async Task<Order?> GetOrderAsync(long id)
    {
        var key = GetOrderCacheKey(id);
        var cachedData = await ExecuteCacheOperationAsync(async () => await _cache.GetStringAsync(key));

        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            return JsonSerializer.Deserialize<Order>(cachedData, _jsonOptions);
        }

        var order = await _innerRepository.GetOrderAsync(id);

        if (order is not null)
        {
            _ = SaveToCacheAsync(order);
        }

        return order;
    }

    /// <inheritdoc />
    public async Task<Order> AddAsync(Order order)
    {
        var createdOrder = await _innerRepository.AddAsync(order);
        await RemoveFromCacheAsync(default, createdOrder.PatientId);
        return createdOrder;
    }

    /// <inheritdoc />
    public async Task<Order> UpdateMessageAsync(Order order)
    {
        var updatedOrder = await _innerRepository.UpdateMessageAsync(order);
        await RemoveFromCacheAsync(order.Id, updatedOrder.PatientId);
        return updatedOrder!;
    }

    /// <inheritdoc />
    public async Task<Order?> UpdateAsync(long id, string message, DateTimeOffset updatedAt)
    {
        var updatedOrder = await _innerRepository.UpdateAsync(id, message, updatedAt);

        if (updatedOrder is null)
        {
            return default;
        }

        await RemoveFromCacheAsync(id, updatedOrder.PatientId);
        return updatedOrder;
    }

    /// <summary>
    /// Gets the cache key for the patient with the specified ID.
    /// </summary>
    /// <param name="patientId">The id of the patient.</param>
    /// <returns>The cache key for the patient.</returns>
    private static string GetPatientCacheKey(long patientId) =>
        $"patient:{patientId}";

    /// <summary>
    /// Gets the cache key for the existence of an order with the specified ID.
    /// </summary>
    /// <param name="id">The id of the order.</param>
    /// <returns>The cache key for the existence of the order.</returns>
    private static string GetOrderCacheKey(long id) =>
        $"order:{id}";

    /// <summary>
    /// Executes a cache operation with error handling.
    /// </summary>
    /// <param name="cacheTask">The cache operation to execute.</param>
    /// <param name="defaultValue">The default value to return if the cache operation fails.</param>
    /// <typeparam name="T">The type of the value returned by the cache operation.</typeparam>
    /// <returns>The return value of the cache operation, or the default value if the operation fails.</returns>
    private async Task<T?> ExecuteCacheOperationAsync<T>(Func<Task<T>> cacheTask, T? defaultValue = default)
    {
        try
        {
            if (!_redisConnection.IsConnected)
            {
                return defaultValue;
            }

            return await _retryPolicy.ExecuteAsync(cacheTask);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "An error occurred during cache operation, bypassing to database.");
            return defaultValue;
        }
    }

    /// <summary>
    /// Saves the specified order to the cache.
    /// </summary>
    /// <param name="order">The order to save to the cache.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async ValueTask SaveToCacheAsync(Order order)
    {
        var key = GetOrderCacheKey(order.Id);
        var json = JsonSerializer.Serialize(order, _jsonOptions);

        await ExecuteCacheOperationAsync(async () =>
        {
            await _cache.SetStringAsync(key, json, _cacheOptions.CurrentValue);
            return true;
        });
    }

    /// <summary>
    /// Removes the specified order from the cache.
    /// </summary>
    /// <param name="orderId">The id of the order to remove from the cache.</param>
    /// <param name="patientId">The id of the patient associated with the order.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async ValueTask RemoveFromCacheAsync(long orderId, long patientId)
    {
        var patientCacheKey = GetPatientCacheKey(patientId);
        var orderCacheKey = GetOrderCacheKey(orderId);

        var tasks = new List<Task>(2);

        if (orderId > 0)
        {
            var removeOrderTask = ExecuteCacheOperationAsync(async () =>
            {
                await _cache.RemoveAsync(orderCacheKey);
                return true;
            });

            tasks.Add(removeOrderTask);
        }

        if (patientId > 0)
        {
            var removePatientTask = ExecuteCacheOperationAsync(async () =>
            {
                await _cache.RemoveAsync(patientCacheKey);
                return true;
            });

            tasks.Add(removePatientTask);
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks);
        }
    }
}
