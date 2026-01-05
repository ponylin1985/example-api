import Redis, { RedisOptions } from "ioredis";
import logger from "./logger";

/**
 * Redis client configuration and instance.
 */
class RedisClient {
  private static instance: Redis | null = null;

  /**
   * Gets the Redis client instance (singleton).
   * @returns The Redis client instance.
   */
  public static getInstance(): Redis {
    if (!RedisClient.instance) {
      const redisPassword = process.env.REDIS_PASSWORD;
      const redisHost = process.env.REDIS_HOST || "localhost";
      const redisPort = parseInt(process.env.REDIS_PORT || "6379", 10);
      const redisDb = parseInt(process.env.REDIS_DB || "0", 10);

      logger.info(
        `Initializing Redis client - Host: ${redisHost}, Port: ${redisPort}, DB: ${redisDb}, Password: ${redisPassword ? "***" : "not set"}`
      );

      const options: RedisOptions = {
        host: redisHost,
        port: redisPort,
        password: redisPassword,
        db: redisDb,
        retryStrategy: (times: number) => {
          const delay = Math.min(times * 50, 2000);
          logger.warn(`Redis connection retry attempt ${times}, waiting ${delay}ms`);
          return delay;
        },
        maxRetriesPerRequest: 3,
        enableReadyCheck: true,
        lazyConnect: false,
      };

      RedisClient.instance = new Redis(options);

      RedisClient.instance.on("connect", () => {
        logger.info("Redis client connected");
      });

      RedisClient.instance.on("ready", () => {
        logger.info("Redis client ready");
      });

      RedisClient.instance.on("error", (err: Error) => {
        logger.error("Redis client error:", err);
      });

      RedisClient.instance.on("close", () => {
        logger.warn("Redis client connection closed");
      });

      RedisClient.instance.on("reconnecting", () => {
        logger.info("Redis client reconnecting");
      });
    }

    return RedisClient.instance;
  }

  /**
   * Closes the Redis connection.
   */
  public static async close(): Promise<void> {
    if (RedisClient.instance) {
      await RedisClient.instance.quit();
      RedisClient.instance = null;
      logger.info("Redis client disconnected");
    }
  }
}

// Create a proxy object that lazily initializes the RedisClient
const redisClientProxy = new Proxy({} as Redis, {
  get(target, prop) {
    const instance = RedisClient.getInstance();
    const value = instance[prop as keyof Redis];
    return typeof value === "function" ? value.bind(instance) : value;
  },
});

export default redisClientProxy;
