/**
 * Utility functions for handling dates consistently in UTC.
 */
export class DateUtils {
  /**
   * Gets the current UTC date time.
   * @returns The current date in UTC.
   */
  static utcNow(): Date {
    return new Date();
  }

  /**
   * Parses a date string as UTC.
   * If the string doesn't contain timezone info, assumes UTC.
   * @param dateString - The date string to parse.
   * @returns The parsed date in UTC, or undefined if invalid.
   */
  static parseUtc(dateString: string): Date | undefined {
    if (!dateString) {
      return undefined;
    }

    // If string doesn't have timezone suffix, append 'Z' to treat as UTC
    const normalized =
      dateString.includes("Z") || dateString.includes("+") || dateString.includes("-") ? dateString : `${dateString}Z`;

    const date = new Date(normalized);

    // Check if date is valid
    return isNaN(date.getTime()) ? undefined : date;
  }

  /**
   * Converts a date to ISO string in UTC format.
   * @param date - The date to convert.
   * @returns ISO string in UTC format.
   */
  static toUtcIsoString(date: Date): string {
    return date.toISOString();
  }

  /**
   * Creates a date from UTC components.
   * @param year - The year.
   * @param month - The month (1-12).
   * @param day - The day.
   * @param hour - The hour (default: 0).
   * @param minute - The minute (default: 0).
   * @param second - The second (default: 0).
   * @returns A date in UTC.
   */
  static createUtc(year: number, month: number, day: number, hour = 0, minute = 0, second = 0): Date {
    return new Date(Date.UTC(year, month - 1, day, hour, minute, second));
  }
}
