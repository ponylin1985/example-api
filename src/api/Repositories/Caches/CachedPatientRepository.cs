using Example.Api.Models;
using Microsoft.Extensions.Caching.Distributed;
using Polly;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    private readonly IPatientRepository _inner;

    /// <summary>
    /// The distributed cache used to store patient data.
    /// </summary>
    private readonly IDistributedCache _cache;

    /// <summary>
    /// The cache entry options used to configure the cache expiration.
    /// </summary>
    private readonly DistributedCacheEntryOptions _cacheOptions;

    /// <summary>
    /// The retry policy for cache operations.
    /// </summary>
    private readonly AsyncRetryPolicy _retryPolicy;

    /// <summary>
    /// JSON serializer options to handle reference loops.
    /// </summary>
    /// <returns></returns>
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedPatientRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger for the CachedPatientRepository.</param>
    /// <param name="inner">The inner patient repository to which calls are delegated.</param>
    /// <param name="cache">The distributed cache used to store patient data.</param>
    public CachedPatientRepository(
        ILogger<CachedPatientRepository> logger,
        IPatientRepository inner,
        IDistributedCache cache)
    {
        _logger = logger;
        _inner = inner;
        _cache = cache;
        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        };

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

        var patient = await _inner.GetPatientAsync(id);

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
        var cachedData = await _cache.GetStringAsync(key);

        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            return true;
        }

        var exists = await _inner.IsExistPatientAsync(id);
        return exists;
    }

    /// <inheritdoc />
    public Task<(IEnumerable<Patient> Data, long TotalCount)> GetPatientsAsync(DateTimeOffset startTime, DateTimeOffset endTime, int pageNumber, int pageSize)
    {
        return _inner.GetPatientsAsync(startTime, endTime, pageNumber, pageSize);
    }

    /// <inheritdoc />
    public Task<Patient?> GetPatientByNameAsync(string name)
    {
        return _inner.GetPatientByNameAsync(name);
    }

    /// <inheritdoc />
    public Task<Patient> CreatePatientAsync(Patient patient)
    {
        return _inner.CreatePatientAsync(patient);
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
