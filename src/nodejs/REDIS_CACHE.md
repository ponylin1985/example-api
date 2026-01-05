# Redis Cache Implementation Guide

This document explains how to use Redis caching in the Node.js Express application.

## Installation

The project uses **`ioredis`** as the Redis client library. It's already included in package.json.

To install dependencies:

```bash
npm install
```

## Configuration

### Environment Variables

Configure Redis connection and cache behavior in your `.env` file:

```env
# Redis configuration
REDIS_HOST=localhost
REDIS_PORT=6379
REDIS_PASSWORD=Admin123
REDIS_DB=0

# Cache configuration
CACHE_SLIDING_EXPIRATION_MINUTES=10
CACHE_ABSOLUTE_EXPIRATION_MINUTES=60
```

### Cache Options

- **CACHE_SLIDING_EXPIRATION_MINUTES**: Extends cache lifetime on each access (default: 10 minutes)
- **CACHE_ABSOLUTE_EXPIRATION_MINUTES**: Maximum cache lifetime regardless of access (default: 60 minutes)

## Usage

### Basic Repository Pattern

#### Without Cache

```typescript
import { OrderRepository } from "./repositories/OrderRepository";

const orderRepository = new OrderRepository();
const order = await orderRepository.getOrderAsync(1);
```

#### With Cache (Decorator Pattern)

```typescript
import { OrderRepository } from "./repositories/OrderRepository";
import { CachedOrderRepository } from "./repositories/caches/CachedOrderRepository";

// Wrap the base repository with caching
const baseRepository = new OrderRepository();
const cachedRepository = new CachedOrderRepository(baseRepository);

// This will check cache first, then database if not found
const order = await cachedRepository.getOrderAsync(1);
```

### Custom Cache Options

```typescript
import { CachedOrderRepository } from "./repositories/caches/CachedOrderRepository";
import { OrderRepository } from "./repositories/OrderRepository";

const customCacheOptions = {
  slidingExpirationMinutes: 5,
  absoluteExpirationRelativeToNowMinutes: 30,
};

const cachedRepository = new CachedOrderRepository(
  new OrderRepository(),
  customCacheOptions
);
```

### Updating Services

Update your services to use the cached repository:

```typescript
// Before
import { OrderRepository } from "../repositories/OrderRepository";

export class OrderService {
  constructor() {
    this.orderRepository = new OrderRepository();
  }
}

// After
import { OrderRepository } from "../repositories/OrderRepository";
import { CachedOrderRepository } from "../repositories/caches/CachedOrderRepository";

export class OrderService {
  constructor() {
    const baseRepository = new OrderRepository();
    this.orderRepository = new CachedOrderRepository(baseRepository);
  }
}
```

## Cache Behavior

### Cache Keys

- **Order**: `order:{id}` - Individual order cache
- **Patient**: `patient:{id}` - Related patient cache

### Cache Operations

| Operation                             | Cache Behavior                                                    |
| ------------------------------------- | ----------------------------------------------------------------- |
| `getOrderAsync(id)`                   | Read from cache first; cache miss reads from DB and caches result |
| `addAsync(order)`                     | Invalidates patient cache after creating order                    |
| `updateAsync(id, message, updatedAt)` | Invalidates order and patient cache after update                  |

### Retry Logic

Cache write operations include automatic retry with exponential backoff:
- Maximum 3 retry attempts
- Exponential delay: 2^attempt seconds + random jitter (0-1000ms)
- Logs warnings on retry and errors on final failure

## Direct Redis Access

For advanced scenarios, you can access the Redis client directly:

```typescript
import redisClient from "./utils/redisClient";

// Set a value
await redisClient.set("key", "value");

// Get a value
const value = await redisClient.get("key");

// Set with expiration (seconds)
await redisClient.setex("key", 300, "value"); // 5 minutes

// Delete a key
await redisClient.del("key");

// Check if key exists
const exists = await redisClient.exists("key");
```

## Why ioredis?

We chose **ioredis** because it offers:

✅ **Full TypeScript support** with comprehensive type definitions  
✅ **Native async/await and Promise support**  
✅ **High performance** and production-ready stability  
✅ **Redis Cluster and Sentinel support** for scaling  
✅ **Robust error handling** and connection management  
✅ **Active community** and regular updates  
✅ **Rich feature set** (Lua scripts, pipelines, pub/sub, streams)

## Starting Redis

### Using Docker Compose

The project includes a Redis Docker Compose configuration:

```bash
# Check docker/redis-docker-compose.yml if it exists
# Or use the existing task
npm run docker:start-redis
```

### Manual Docker

```bash
docker run -d \
  --name redis \
  -p 6379:6379 \
  -e REDIS_PASSWORD=Admin123 \
  redis:latest \
  redis-server --requirepass Admin123
```

### Local Installation

```bash
# macOS
brew install redis
brew services start redis

# Ubuntu
sudo apt-get install redis-server
sudo systemctl start redis
```

## Monitoring

The Redis client logs connection events:

- **connect**: Redis client connected
- **ready**: Redis client ready for operations
- **error**: Connection or operation errors
- **close**: Connection closed
- **reconnecting**: Attempting to reconnect

Check your logs to monitor Redis health and cache operations.

## Best Practices

1. **Use cache for read-heavy operations** - Frequent reads benefit most from caching
2. **Invalidate cache on writes** - Always clear related cache on data modifications
3. **Set appropriate TTLs** - Balance between data freshness and cache hit rate
4. **Monitor cache hit ratio** - Track effectiveness of your caching strategy
5. **Handle cache failures gracefully** - Application should work even if Redis is down
6. **Don't cache sensitive data** - Be mindful of what data goes into cache

## Troubleshooting

### Redis Connection Failed

Check if Redis is running:
```bash
redis-cli ping
# Should return: PONG
```

### Cache Not Working

1. Check environment variables are loaded
2. Verify LOG_LEVEL=debug to see cache operations
3. Check Redis connection logs in application output

### Performance Issues

1. Adjust TTL values in cache options
2. Monitor Redis memory usage: `redis-cli info memory`
3. Consider Redis eviction policies if memory is full
