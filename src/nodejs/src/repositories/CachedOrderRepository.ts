import { CacheEntryOptions, defaultCacheOptions } from "../config/CacheEntryOptions";
import { IOrderRepository } from "./IOrderRepository";
import { JsonUtils } from "../utils/jsonUtils";
import { Order } from "../entities/Order";
import logger from "../utils/logger";
import redisClient from "../utils/redisClient";

/**
 * Decorator for IOrderRepository that adds Redis caching functionality.
 * Implements the Decorator pattern to wrap an existing repository with caching capabilities.
 */
export class CachedOrderRepository implements IOrderRepository {
  /**
   * Maximum number of retry attempts for cache operations.
   */
  private readonly maxRetries = 3;

  /**
   * Creates a new instance of CachedOrderRepository.
   * @param innerRepository - The repository to decorate with caching.
   * @param cacheOptions - Optional cache configuration options.
   */
  constructor(
    private innerRepository: IOrderRepository,
    private cacheOptions: CacheEntryOptions = defaultCacheOptions
  ) {
    this.innerRepository = innerRepository;
    this.cacheOptions = cacheOptions;
  }

  /**
   * Retrieves an order by its ID, using cache when available.
   * @param id - The order ID.
   * @returns The order if found, null otherwise.
   */
  async getOrderAsync(id: number): Promise<Order | null> {
    const key = this.getOrderCacheKey(id);

    try {
      const cachedData = await redisClient.get(key);

      if (cachedData) {
        logger.debug(`Cache hit for order: ${id}`);
        const data = JsonUtils.deserialize<Order>(cachedData);
        if (data) {
          return Object.assign(new Order(), data);
        }
        return null;
      }

      logger.debug(`Cache miss for order: ${id}`);
    } catch (error) {
      logger.warn(`Failed to read from cache for order ${id}:`, error);
    }

    const order = await this.innerRepository.getOrderAsync(id);

    if (order) {
      await this.saveToCacheAsync(order);
    }

    return order;
  }

  /**
   * Adds a new order to the repository.
   * @param order - The order to add.
   * @returns The added order with generated ID.
   */
  async addAsync(order: Order): Promise<Order> {
    const createdOrder = await this.innerRepository.addAsync(order);
    await this.removeFromCacheAsync(undefined, createdOrder.patientId);
    return createdOrder;
  }

  /**
   * Updates an order's message.
   * @param id - The order ID.
   * @param message - The new message.
   * @param updatedAt - The update timestamp.
   * @returns The updated order if found, null otherwise.
   */
  async updateAsync(id: number, message: string, updatedAt: Date): Promise<Order | null> {
    const updatedOrder = await this.innerRepository.updateAsync(id, message, updatedAt);

    if (updatedOrder) {
      await this.removeFromCacheAsync(id, updatedOrder.patientId);
    }

    return updatedOrder;
  }

  /**
   * Saves an order to the cache with retry logic.
   * @param order - The order to cache.
   */
  private async saveToCacheAsync(order: Order): Promise<void> {
    const key = this.getOrderCacheKey(order.id);
    const value = JSON.stringify(order);
    const ttl = this.getTtlInSeconds();

    for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
      try {
        if (ttl > 0) {
          await redisClient.setex(key, ttl, value);
        } else {
          await redisClient.set(key, value);
        }
        logger.debug(`Cached order: ${order.id}`);
        return;
      } catch (error) {
        const delay = Math.pow(2, attempt) * 1000 + Math.random() * 1000;
        logger.warn(
          `Cache write failed for order ${order.id}. Attempt ${attempt}/${this.maxRetries}. Retrying in ${delay}ms`,
          error
        );

        if (attempt < this.maxRetries) {
          await this.delayAsync(delay);
        } else {
          logger.error(`Failed to cache order ${order.id} after ${this.maxRetries} attempts`, error);
        }
      }
    }
  }

  /**
   * Removes cache entries related to an order.
   * @param orderId - The order ID to remove from cache (optional).
   * @param patientId - The patient ID to remove related cache (optional).
   */
  private async removeFromCacheAsync(orderId?: number, patientId?: number): Promise<void> {
    try {
      const keysToDelete: string[] = [];

      if (orderId !== undefined) {
        keysToDelete.push(this.getOrderCacheKey(orderId));
      }

      if (patientId !== undefined) {
        keysToDelete.push(this.getPatientCacheKey(patientId));
      }

      if (keysToDelete.length > 0) {
        await redisClient.del(...keysToDelete);
        logger.debug(`Removed cache keys: ${keysToDelete.join(", ")}`);
      }
    } catch (error) {
      logger.warn("Failed to remove cache entries:", error);
    }
  }

  /**
   * Gets the cache key for an order.
   * @param id - The order ID.
   * @returns The cache key.
   */
  private getOrderCacheKey(id: number): string {
    return `order:${id}`;
  }

  /**
   * Gets the cache key for a patient.
   * @param patientId - The patient ID.
   * @returns The cache key.
   */
  private getPatientCacheKey(patientId: number): string {
    return `patient:${patientId}`;
  }

  /**
   * Calculates the TTL (Time To Live) in seconds based on cache options.
   * @returns The TTL in seconds.
   */
  private getTtlInSeconds(): number {
    const { slidingExpirationMinutes, absoluteExpirationRelativeToNowMinutes } = this.cacheOptions;

    // Use the smaller of the two expiration times if both are set
    if (slidingExpirationMinutes && absoluteExpirationRelativeToNowMinutes) {
      return Math.min(slidingExpirationMinutes, absoluteExpirationRelativeToNowMinutes) * 60;
    }

    if (slidingExpirationMinutes) {
      return slidingExpirationMinutes * 60;
    }

    if (absoluteExpirationRelativeToNowMinutes) {
      return absoluteExpirationRelativeToNowMinutes * 60;
    }

    return 0;
  }

  /**
   * Sleep for a specified duration.
   * @param ms - Duration in milliseconds.
   */
  private delayAsync(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }
}
