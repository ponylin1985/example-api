/**
 * Options for distributed cache entries.
 */
export interface CacheEntryOptions {
  /**
   * Sliding expiration in minutes.
   * Resets the expiration time when the cache item is accessed.
   */
  slidingExpirationMinutes?: number;

  /**
   * Absolute expiration relative to now in minutes.
   * The cache item will expire after this duration, regardless of access.
   */
  absoluteExpirationRelativeToNowMinutes?: number;
}

/**
 * Default cache entry options.
 */
export const defaultCacheOptions: CacheEntryOptions = {
  slidingExpirationMinutes: parseInt(process.env.CACHE_SLIDING_EXPIRATION_MINUTES || "10", 10),
  absoluteExpirationRelativeToNowMinutes: parseInt(process.env.CACHE_ABSOLUTE_EXPIRATION_MINUTES || "60", 10),
};
