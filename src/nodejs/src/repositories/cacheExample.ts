/**
 * Example: How to use Cached Repositories
 *
 * This file demonstrates how to integrate Redis caching with your repositories.
 */

import { OrderRepository } from "./OrderRepository";
import { PatientRepository } from "./PatientRepository";
import { CachedOrderRepository } from "./CachedOrderRepository";
import { CachedPatientRepository } from "./CachedPatientRepository";
import { CacheEntryOptions } from "../config/CacheEntryOptions";

// ========================================
// Example 1: Basic Usage with Default Options
// ========================================

export function createOrderRepositoryWithCache(): CachedOrderRepository {
  const baseRepository = new OrderRepository();
  return new CachedOrderRepository(baseRepository);
}

export function createPatientRepositoryWithCache(): CachedPatientRepository {
  const baseRepository = new PatientRepository();
  return new CachedPatientRepository(baseRepository);
}

// ========================================
// Example 2: Custom Cache Options
// ========================================

export function createOrderRepositoryWithCustomCache(): CachedOrderRepository {
  const baseRepository = new OrderRepository();
  const customOptions: CacheEntryOptions = {
    slidingExpirationMinutes: 5, // Cache expires after 5 minutes of no access
    absoluteExpirationRelativeToNowMinutes: 30, // Maximum 30 minutes in cache
  };
  return new CachedOrderRepository(baseRepository, customOptions);
}

export function createPatientRepositoryWithCustomCache(): CachedPatientRepository {
  const baseRepository = new PatientRepository();
  const customOptions: CacheEntryOptions = {
    slidingExpirationMinutes: 15, // Patients cache longer
    absoluteExpirationRelativeToNowMinutes: 60,
  };
  return new CachedPatientRepository(baseRepository, customOptions);
}

// ========================================
// Example 3: Using in a Service
// ========================================

/**
 * Example of how to modify Services to use caching.
 *
 * OrderService:
 * ```typescript
 * constructor() {
 *   const baseRepository = new OrderRepository();
 *   this.orderRepository = new CachedOrderRepository(baseRepository);
 * }
 * ```
 *
 * PatientService:
 * ```typescript
 * constructor() {
 *   const baseRepository = new PatientRepository();
 *   this.patientRepository = new CachedPatientRepository(baseRepository);
 * }
 * ```
 */

// ========================================
// Example 4: Usage Demonstration - Order
// ========================================

/**
 * Demonstrates cache behavior with Order operations.
 */
export async function demonstrateOrderCacheBehavior() {
  const repository = createOrderRepositoryWithCache();

  // First call - Cache miss, reads from database
  console.log("First call - should be cache miss");
  const order1 = await repository.getOrderAsync(1);
  console.log("Order:", order1);

  // Second call - Cache hit, reads from Redis
  console.log("\nSecond call - should be cache hit");
  const order2 = await repository.getOrderAsync(1);
  console.log("Order:", order2);

  // Update - Invalidates cache
  console.log("\nUpdating order - invalidates cache");
  await repository.updateAsync(1, "Updated message", new Date());

  // Third call - Cache miss again after invalidation
  console.log("\nThird call - should be cache miss (cache invalidated)");
  const order3 = await repository.getOrderAsync(1);
  console.log("Order:", order3);
}

// ========================================
// Example 5: Usage Demonstration - Patient
// ========================================

/**
 * Demonstrates cache behavior with Patient operations.
 */
export async function demonstratePatientCacheBehavior() {
  const repository = createPatientRepositoryWithCache();

  // First call - Cache miss, reads from database
  console.log("First call - should be cache miss");
  const patient1 = await repository.getPatientAsync(1);
  console.log("Patient:", patient1);

  // Second call - Cache hit, reads from Redis
  console.log("\nSecond call - should be cache hit");
  const patient2 = await repository.getPatientAsync(1);
  console.log("Patient:", patient2);

  // Check existence - Uses cache
  console.log("\nChecking existence - should use cache");
  const exists = await repository.isExistPatientAsync(1);
  console.log("Patient exists:", exists);

  // Query by name - Bypasses cache
  console.log("\nQuery by name - bypasses cache");
  const patientByName = await repository.getPatientByNameAsync("John Doe");
  console.log("Patient by name:", patientByName);
}

// ========================================
// Example 6: Direct Redis Usage
// ========================================

/**
 * Example of using Redis directly for custom caching needs.
 */
export async function customCacheExample() {
  const redisClient = (await import("../utils/redisClient")).default;

  // Set a custom cache value
  await redisClient.set("custom:key", JSON.stringify({ data: "value" }));

  // Set with expiration (300 seconds = 5 minutes)
  await redisClient.setex("custom:key:ttl", 300, JSON.stringify({ data: "value" }));

  // Get a value
  const value = await redisClient.get("custom:key");
  console.log("Cached value:", value);

  // Delete a key
  await redisClient.del("custom:key");

  // Check if key exists
  const exists = await redisClient.exists("custom:key:ttl");
  console.log("Key exists:", exists === 1);
}

/**
 * Note: Uncomment the following lines to run the examples:
 *
 * demonstrateOrderCacheBehavior().catch(console.error);
 * demonstratePatientCacheBehavior().catch(console.error);
 * customCacheExample().catch(console.error);
 */
