using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisClient.Services;

/// <summary>
/// Provides a service for caching and retrieving data in Redis.
/// </summary>
public class RedisService : IRedisService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisService"/> class.
    /// </summary>
    /// <param name="cache">The distributed cache instance.</param>
    /// <param name="logger">The logger for logging cache-related operations.</param>
    public RedisService(IDistributedCache cache, ILogger<RedisService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// Sets a key-value pair in the cache with a specified expiration time.
    /// </summary>
    /// <typeparam name="T">The type of data to be cached.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="data">The data to be cached.</param>
    /// <param name="cacheDuration">The duration for which the data should be cached.</param>
    /// <param name="token">A cancellation token.</param>
    public async Task Set<T>(string key, T data, TimeSpan cacheDuration, CancellationToken token = default)
    {
        try
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }
            
            if (cacheDuration <= TimeSpan.Zero)
            {
                throw new ArgumentException("Cache duration must be a positive time span.", nameof(cacheDuration));
            }

            var cachedData = await _cache.GetStringAsync(key, token);

            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Key {K}, is already in cache", key);
                return;
            }
            
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };

            string json = JsonSerializer.Serialize(data);

            await _cache.SetStringAsync(key, json, options, token);
            
            _logger.LogInformation("Key {K}, was successfully stored in cache", key);
        }
        
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid argument when setting cache. Key: {Key}", key);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while setting cache. Key: {Key}", key);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a cached value by its key.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>The cached data, or the default value of <typeparamref name="T"/> if not found.</returns>
    public async Task<T?> Get<T>(string key, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key argument can't be null when getting cache");
        }
        
        try
        {
            var resultString = await _cache.GetStringAsync(key, token);

            if (string.IsNullOrEmpty(resultString))
            {
                _logger.LogWarning("There is no data in cache for key: {K}", key);
                return default;
            }

            return JsonSerializer.Deserialize<T>(resultString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while retrieving from cache. Key: {Key}", key);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing cached value with a new value and sets its expiration time.
    /// </summary>
    /// <typeparam name="T">The type of data to be updated.</typeparam>
    /// <param name="key">The cache key of the value to update.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="cacheDuration">The new duration for which the data should be cached.</param>
    /// <param name="token">A cancellation token.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty, or when <paramref name="cacheDuration"/> is not a positive time span.</exception>
    public async Task Update<T>(string key, T value, TimeSpan cacheDuration, CancellationToken token = default)
    {
        if(string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
            
        if (cacheDuration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Cache duration must be a positive time span.", nameof(cacheDuration));
        }

        try
        {
            string? currentValue = await _cache.GetStringAsync(key, token);

            if (!string.IsNullOrEmpty(currentValue))
            {
                await Set(key, value, cacheDuration, token);
            }
            else
            {
                throw new InvalidOperationException($"No record to update with key: {key}");
            }
        }
        
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating data in cache. Key: {Key}", key);
            throw;
        }
        
        
    }

    /// <summary>
    /// Deletes a cached value by its key.
    /// </summary>
    /// <param name="key">The cache key to delete.</param>
    /// <param name="token">A cancellation token.</param>
    public async Task Delete(string key, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key argument can't be null when deleting from cache");
        }

        try
        {
            await _cache.RemoveAsync(key, token);

            _logger.LogInformation("Record was removed from cache. Key: {K}", key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting cache. Key: {Key}", key);
            throw;
        }
    }
}