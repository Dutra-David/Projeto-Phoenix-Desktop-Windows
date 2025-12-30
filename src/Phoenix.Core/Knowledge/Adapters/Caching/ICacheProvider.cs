using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phoenix.Core.Knowledge.Adapters.Caching
{
    /// <summary>
    /// Provides caching abstraction for knowledge items with TTL support.
    /// Enables easy switching between memory and distributed cache implementations.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Get a value from cache by key.
        /// </summary>
        /// <typeparam name="T">Type of the cached value</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached value or null if not found or expired</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Set a value in cache with optional TTL.
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="ttl">Time to live. If null, uses default TTL</param>
        Task SetAsync<T>(string key, T value, TimeSpan? ttl = null);

        /// <summary>
        /// Get value from cache or execute factory and cache the result.
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Factory function to generate value if not cached</param>
        /// <param name="ttl">Time to live for cached result</param>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null);

        /// <summary>
        /// Remove a value from cache.
        /// </summary>
        /// <param name="key">Cache key</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Remove all values matching a pattern from cache.
        /// </summary>
        /// <param name="pattern">Pattern to match (e.g., "user:*")</param>
        Task RemoveByPatternAsync(string pattern);

        /// <summary>
        /// Clear entire cache.
        /// </summary>
        Task ClearAsync();

        /// <summary>
        /// Check if a key exists in cache.
        /// </summary>
        /// <param name="key">Cache key</param>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Get time remaining until key expires.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>TimeSpan remaining or null if not set or expired</returns>
        Task<TimeSpan?> GetTimeToLiveAsync(string key);

        /// <summary>
        /// Get cache statistics for monitoring.
        /// </summary>
        /// <returns>Cache statistics including hits, misses, and size info</returns>
        Task<CacheStatistics> GetStatisticsAsync();

        /// <summary>
        /// Set expiration for an existing key.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="ttl">Time to live</param>
        Task SetExpireAsync(string key, TimeSpan ttl);
    }

    /// <summary>
    /// Cache operation statistics for monitoring and diagnostics.
    /// </summary>
    public class CacheStatistics
    {
        /// <summary>
        /// Number of cache hits (successful retrievals).
        /// </summary>
        public long Hits { get; set; }

        /// <summary>
        /// Number of cache misses (failed retrievals).
        /// </summary>
        public long Misses { get; set; }

        /// <summary>
        /// Current number of entries in cache.
        /// </summary>
        public long EntryCount { get; set; }

        /// <summary>
        /// Estimated size of cache in bytes.
        /// </summary>
        public long SizeBytes { get; set; }

        /// <summary>
        /// Hit ratio (hits / (hits + misses)).
        /// </summary>
        public double HitRatio => Hits + Misses > 0 ? (double)Hits / (Hits + Misses) : 0;

        /// <summary>
        /// Time the cache was last cleared.
        /// </summary>
        public DateTime? LastClearedAt { get; set; }

        /// <summary>
        /// Timestamp when statistics were collected.
        /// </summary>
        public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    }
}
