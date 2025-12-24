using Example.Api.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using StackExchange.Redis;
using System.Text.Json;

namespace Example.Api.Repositories.Caches;

/// <summary>
/// Decorator for IPatientRepository that adds caching.
/// </summary>
public class CachedPatientRepository : IPatientRepository
{
    /// <summary>
    /// Logger for the CachedPatientRepository.
    /// </summary>
    private readonly ILogger<CachedPatientRepository> _logger;

    /// <summary>
    /// The inner patient repository to which calls are delegated.
    /// </summary>
    private readonly IPatientRepository _innerRepository;

    /// <summary>
    /// The distributed cache used to store patient data.
    /// </summary>
    private readonly IDistributedCache _cache;

    /// <summary>
    /// The cache entry options used to configure the cache expiration.
    /// </summary>
    private readonly DistributedCacheEntryOptions _cacheOptions;

    /// <summary>
    /// The Redis database instance.
    /// </summary>
    private readonly IDatabase _redisDb;

    /// <summary>
    /// The retry policy for cache operations.
    /// </summary>
    private readonly AsyncRetryPolicy _retryPolicy;

    /// <summary>
    /// JSON serializer options to handle reference loops.
    /// </summary>
    /// <returns></returns>
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedPatientRepository"/> class.
    /// </summary>
    /// <param name="logger">Logger for the CachedPatientRepository.</param>
    /// <param name="innerRepository">The inner patient repository to which calls are delegated.</param>
    /// <param name="cache">The distributed cache used to store patient data.</param>
    /// <param name="cacheOptions">Cache entry options to configure cache expiration.</param>
    /// <param name="jsonOptions">JSON serializer options to handle reference loops.</param>
    /// <param name="redis">The Redis connection multiplexer.</param>
    public CachedPatientRepository(
        ILogger<CachedPatientRepository> logger,
        IPatientRepository innerRepository,
        IDistributedCache cache,
        IOptions<DistributedCacheEntryOptions> cacheOptions,
        JsonSerializerOptions jsonOptions,
        IConnectionMultiplexer redis)
    {
        _logger = logger;
        _innerRepository = innerRepository;
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
        _jsonOptions = jsonOptions;
        _redisDb = redis.GetDatabase();

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
                    _logger.LogWarning(exception,
                        "Cache write failed. Retrying in {TimeSpan}. Attempt {RetryCount}.",
                            timeSpan,
                            retryCount);
                });
    }

    /// <inheritdoc />
    public async Task<Patient?> GetPatientAsync(long id)
    {
        string key = GetExistenceCacheKey(id);
        var cachedData = await _cache.GetStringAsync(key);

        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            return JsonSerializer.Deserialize<Patient>(cachedData, _jsonOptions);
        }

        var patient = await _innerRepository.GetPatientAsync(id);

        if (patient is not null)
        {
            await SaveToCacheAsync(patient);
        }

        return patient;
    }

    /// <inheritdoc />
    public async Task<bool> IsExistPatientAsync(long id)
    {
        string key = GetExistenceCacheKey(id);

        if (await _redisDb.KeyExistsAsync(key))
        {
            return true;
        }

        var exists = await _innerRepository.IsExistPatientAsync(id);
        return exists;
    }

    /// <inheritdoc />
    public Task<(IEnumerable<Patient> Data, long TotalCount)> GetPatientsAsync(
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int pageNumber,
        int pageSize)
    {
        return _innerRepository.GetPatientsAsync(startTime, endTime, pageNumber, pageSize);
    }

    /// <inheritdoc />
    public Task<Patient?> GetPatientByNameAsync(string name)
    {
        return _innerRepository.GetPatientByNameAsync(name);
    }

    /// <inheritdoc />
    public Task<Patient> CreatePatientAsync(Patient patient)
    {
        return _innerRepository.CreatePatientAsync(patient);
    }

    /// <summary>
    /// Gets the cache key for the existence of a patient with the specified ID.
    /// </summary>
    /// <param name="id">The id of the patient.</param>
    /// <returns>The cache key for the existence of the patient.</returns>
    private static string GetExistenceCacheKey(long id) =>
        $"patient:{id}";

    /// <summary>
    /// Saves the specified patient to the cache.
    /// </summary>
    /// <param name="patient">The patient to save to the cache.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SaveToCacheAsync(Patient patient)
    {
        var key = GetExistenceCacheKey(patient.Id);
        var patientJson = JsonSerializer.Serialize(patient, _jsonOptions);

        try
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _cache.SetStringAsync(key, patientJson, _cacheOptions);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving patient to cache with key {Key}", key);
        }
    }
}
