import { CacheEntryOptions, defaultCacheOptions } from "../config/CacheEntryOptions";
import { IPatientRepository } from "./IPatientRepository";
import { Patient } from "../entities/Patient";
import logger from "../utils/logger";
import redisClient from "../utils/redisClient";
import { JsonUtils } from "../utils/jsonUtils";

/**
 * Decorator for IPatientRepository that adds Redis caching functionality.
 * Implements the Decorator pattern to wrap an existing repository with caching capabilities.
 */
export class CachedPatientRepository implements IPatientRepository {
  /**
   * Maximum number of retry attempts for cache operations.
   */
  private readonly maxRetries = 3;

  /**
   * Creates a new instance of CachedPatientRepository.
   * @param innerRepository - The repository to decorate with caching.
   * @param cacheOptions - Optional cache configuration options.
   */
  constructor(
    private innerRepository: IPatientRepository,
    private cacheOptions: CacheEntryOptions = defaultCacheOptions
  ) {
    this.innerRepository = innerRepository;
    this.cacheOptions = cacheOptions;
  }

  /**
   * Retrieves a patient by its ID, using cache when available.
   * @param id - The patient ID.
   * @returns The patient if found, null otherwise.
   */
  async getPatientAsync(id: number): Promise<Patient | null> {
    const key = this.getExistenceCacheKey(id);

    try {
      const cachedData = await redisClient.get(key);

      if (cachedData) {
        logger.debug(`Cache hit for patient: ${id}`);
        const data = JsonUtils.deserialize<Patient>(cachedData);
        if (data) {
          return Object.assign(new Patient(), data);
        }
        return null;
      }

      logger.debug(`Cache miss for patient: ${id}`);
    } catch (error) {
      logger.warn(`Failed to read from cache for patient ${id}:`, error);
    }

    const patient = await this.innerRepository.getPatientAsync(id);

    if (patient) {
      await this.saveToCacheAsync(patient);
    }

    return patient;
  }

  /**
   * Checks if a patient exists by ID, using cache when available.
   * @param id - The patient ID.
   * @returns True if the patient exists, false otherwise.
   */
  async isExistPatientAsync(id: number): Promise<boolean> {
    const key = this.getExistenceCacheKey(id);

    try {
      const exists = await redisClient.exists(key);
      if (exists === 1) {
        logger.debug(`Cache hit for patient existence: ${id}`);
        return true;
      }
    } catch (error) {
      logger.warn(`Failed to check cache for patient existence ${id}:`, error);
    }

    const exists = await this.innerRepository.isExistPatientAsync(id);
    return exists;
  }

  /**
   * Retrieves a paginated list of patients within a date range.
   * Note: This method bypasses cache due to complex query nature.
   * @param startTime - The start of the date range.
   * @param endTime - The end of the date range.
   * @param pageNumber - The page number to retrieve.
   * @param pageSize - The number of items per page.
   * @returns A tuple containing the patients array and total count.
   */
  async getPatientsAsync(
    startTime: Date,
    endTime: Date,
    pageNumber: number,
    pageSize: number
  ): Promise<[Patient[], number]> {
    return this.innerRepository.getPatientsAsync(startTime, endTime, pageNumber, pageSize);
  }

  /**
   * Retrieves a patient by name.
   * Note: This method bypasses cache due to non-ID lookup.
   * @param name - The patient name.
   * @returns The patient if found, null otherwise.
   */
  async getPatientByNameAsync(name: string): Promise<Patient | null> {
    return this.innerRepository.getPatientByNameAsync(name);
  }

  /**
   * Adds a new patient to the repository.
   * @param patient - The patient to add.
   * @returns The added patient with generated ID.
   */
  async addAsync(patient: Patient): Promise<Patient> {
    return this.innerRepository.addAsync(patient);
  }

  /**
   * Gets the cache key for a patient's existence.
   * @param id - The patient ID.
   * @returns The cache key.
   */
  private getExistenceCacheKey(id: number): string {
    return `patient:${id}`;
  }

  /**
   * Saves a patient to the cache with retry logic.
   * @param patient - The patient to cache.
   */
  private async saveToCacheAsync(patient: Patient): Promise<void> {
    const key = this.getExistenceCacheKey(patient.id);
    const value = JSON.stringify(patient);
    const ttl = this.getTtlInSeconds();

    for (let attempt = 1; attempt <= this.maxRetries; attempt++) {
      try {
        if (ttl > 0) {
          await redisClient.setex(key, ttl, value);
        } else {
          await redisClient.set(key, value);
        }
        logger.debug(`Cached patient: ${patient.id}`);
        return;
      } catch (error) {
        const delay = Math.pow(2, attempt) * 1000 + Math.random() * 1000;
        logger.warn(
          `Cache write failed for patient ${patient.id}. Attempt ${attempt}/${this.maxRetries}. Retrying in ${delay}ms`,
          error
        );

        if (attempt < this.maxRetries) {
          await this.delayAsync(delay);
        } else {
          logger.error(`Failed to cache patient ${patient.id} after ${this.maxRetries} attempts`, error);
        }
      }
    }
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
