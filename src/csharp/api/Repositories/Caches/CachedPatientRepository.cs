using Example.Api.Enums;
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
public sealed class CachedPatientRepository : IPatientRepository
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
    private readonly IOptionsMonitor<DistributedCacheEntryOptions> _cacheOptions;

    /// <summary>
    /// The Redis connection multiplexer.
    /// </summary>
    private readonly IConnectionMultiplexer _redisConnection;

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
    /// <param name="redisConnection">The Redis connection multiplexer.</param>
    public CachedPatientRepository(
        ILogger<CachedPatientRepository> logger,
        IPatientRepository innerRepository,
        IDistributedCache cache,
        IOptionsMonitor<DistributedCacheEntryOptions> cacheOptions,
        JsonSerializerOptions jsonOptions,
        IConnectionMultiplexer redisConnection)
    {
        _logger = logger;
        _innerRepository = innerRepository;
        _cache = cache;
        _cacheOptions = cacheOptions;
        _jsonOptions = jsonOptions;
        _redisConnection = redisConnection;

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
    public Task<(IEnumerable<Patient> Data, long TotalCount)> GetPatientsAsync(
        int pageNumber,
        int pageSize,
        string? name = default,
        bool isPrefix = false,
        string? email = default,
        string? phoneNumber = default,
        PatientStatus? status = default,
        DateTimeOffset? startTime = default,
        DateTimeOffset? endTime = default)
    {
        return _innerRepository.GetPatientsAsync(
            pageNumber,
            pageSize,
            name,
            isPrefix,
            email,
            phoneNumber,
            status,
            startTime,
            endTime);
    }

    /// <inheritdoc />
    public async Task<Patient?> GetPatientAsync(long id)
    {
        var key = GetPatientCacheKey(id);
        var cachedData = await ExecuteCacheOperationAsync(
            async () => await _cache.GetStringAsync(key));

        if (!string.IsNullOrWhiteSpace(cachedData))
        {
            return JsonSerializer.Deserialize<Patient>(cachedData, _jsonOptions);
        }

        var patient = await _innerRepository.GetPatientAsync(id);

        if (patient is not null)
        {
            _ = SaveToCacheAsync(patient);
        }

        return patient;
    }

    /// <inheritdoc />
    public async Task<bool> IsExistPatientAsync(long id)
    {
        var key = GetPatientCacheKey(id);
        var existed = await ExecuteCacheOperationAsync(async () =>
            await _redisConnection.GetDatabase().KeyExistsAsync(key), false);

        if (existed)
        {
            return true;
        }

        var exists = await _innerRepository.IsExistPatientAsync(id);
        return exists;
    }

    /// <inheritdoc />
    public Task<bool> IsExistPatientByEmailAsync(string email)
    {
        return _innerRepository.IsExistPatientByEmailAsync(email);
    }

    /// <inheritdoc />
    public Task<bool> IsExistPatientByPhoneAsync(string phoneNumber)
    {
        return _innerRepository.IsExistPatientByPhoneAsync(phoneNumber);
    }

    /// <inheritdoc />
    public Task<Patient?> GetPatientByNameAsync(string name)
    {
        return _innerRepository.GetPatientByNameAsync(name);
    }

    /// <inheritdoc />
    public async Task<Patient> AddAsync(Patient patient)
    {
        var createdPatient = await _innerRepository.AddAsync(patient);

        if (createdPatient is { Id: > 0 })
        {
            _ = RemoveFromCacheAsync(createdPatient.Id);
        }

        return createdPatient;
    }

    /// <inheritdoc />
    public async Task<Patient> UpdateAsync(Patient patient)
    {
        var updatedPatient = await _innerRepository.UpdateAsync(patient);

        if (updatedPatient is { Id: > 0 })
        {
            _ = RemoveFromCacheAsync(updatedPatient.Id);
        }

        return updatedPatient!;
    }

    /// <summary>
    /// Gets the cache key for the existence of a patient with the specified ID.
    /// </summary>
    /// <param name="id">The id of the patient.</param>
    /// <returns>The cache key for the existence of the patient.</returns>
    private static string GetPatientCacheKey(long id) =>
        $"patient:{id}";

    /// <summary>
    /// Saves the specified patient to the cache.
    /// </summary>
    /// <param name="patient">The patient to save to the cache.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SaveToCacheAsync(Patient patient)
    {
        var key = GetPatientCacheKey(patient.Id);
        var patientJson = JsonSerializer.Serialize(patient, _jsonOptions);

        try
        {
            await ExecuteCacheOperationAsync(async () =>
            {
                await _cache.SetStringAsync(key, patientJson, _cacheOptions.CurrentValue);
                return true;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving patient to cache with key {Key}", key);
        }
    }

    /// <summary>
    /// Removes the specified patient from the cache.
    /// </summary>
    /// <param name="patientId">The ID of the patient to remove from the cache.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async ValueTask RemoveFromCacheAsync(long patientId)
    {
        var key = GetPatientCacheKey(patientId);

        await ExecuteCacheOperationAsync(async () =>
        {
            await _cache.RemoveAsync(key);
            return true;
        });
    }

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
}
