namespace RedisClient.Services;

public interface IRedisService
{
    /// <summary>
    /// Sets a key-value pair in the cache with a specified expiration time.
    /// </summary>
    /// <typeparam name="T">The type of data to be cached.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="data">The data to be cached.</param>
    /// <param name="cacheDuration">The duration for which the data should be cached.</param>
    /// <param name="token">A cancellation token.</param>
    Task Set<T>(string key, T data, TimeSpan cacheDuration, CancellationToken token = default);

    /// <summary>
    /// Retrieves a cached value by its key.
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>The cached data, or the default value of <typeparamref name="T"/> if not found.</returns>
    Task<T?> Get<T>(string key, CancellationToken token = default);

    /// <summary>
    /// Updates an existing cached value with a new value and sets its expiration time.
    /// </summary>
    /// <typeparam name="T">The type of data to be updated.</typeparam>
    /// <param name="key">The cache key of the value to update.</param>
    /// <param name="value">The new value to set.</param>
    /// <param name="cacheDuration">The new duration for which the data should be cached.</param>
    /// <param name="token">A cancellation token.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is null or empty, or when <paramref name="cacheDuration"/> is not a positive time span.</exception>
    Task Update<T>(string key, T value, TimeSpan cacheDuration, CancellationToken token = default);

    /// <summary>
    /// Deletes a cached value by its key.
    /// </summary>
    /// <param name="key">The cache key to delete.</param>
    /// <param name="token">A cancellation token.</param>
    Task Delete(string key, CancellationToken token = default);
}