/**
 * Utility class for JSON serialization and deserialization with Date support.
 */
export class JsonUtils {
  /**
   * Serializes an object to JSON string.
   * @param obj - The object to serialize.
   * @returns JSON string representation of the object.
   */
  static serialize(obj: unknown): string {
    return JSON.stringify(obj);
  }

  /**
   * Deserializes a JSON string to an object with automatic Date conversion.
   * Automatically converts ISO 8601 date strings to Date objects.
   * @param jsonString - The JSON string to deserialize.
   * @returns The deserialized object or undefined if parsing fails.
   * @template T - The type of the object to deserialize.
   */
  static deserialize<T>(jsonString: string): T | undefined {
    try {
      return JSON.parse(jsonString, this.dateReviver) as T;
    } catch (error) {
      console.error("Failed to deserialize JSON:", error);
      return undefined;
    }
  }

  /**
   * Reviver function for JSON. parse that converts ISO date strings to Date objects.
   * @param key - The property key.
   * @param value - The property value.
   * @returns The converted value (Date object if applicable, otherwise original value).
   */
  private static dateReviver(_key: string, value: unknown): unknown {
    if (typeof value === "string" && JsonUtils.isIsoDateString(value)) {
      return new Date(value);
    }
    return value;
  }

  /**
   * Checks if a string is an ISO 8601 date string.
   * Matches formats like:
   * - 2024-01-05T12:30:45.123Z
   * - 2024-01-05T12:30:45Z
   * - 2024-01-05T12:30:45.123+08:00
   * @param value - The string to check.
   * @returns True if the string is an ISO date string, false otherwise.
   */
  private static isIsoDateString(value: string): boolean {
    // ISO 8601 date format regex
    const isoDateRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{3})?(Z|[+-]\d{2}:\d{2})?$/;
    return isoDateRegex.test(value);
  }
}
