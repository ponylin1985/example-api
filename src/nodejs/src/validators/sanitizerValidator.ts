import DOMPurify from "dompurify";
import { JSDOM } from "jsdom";

/**
 * Validator for sanitizing input strings to prevent XSS attacks.
 * Uses DOMPurify to detect and prevent malicious HTML/scripts.
 */
export class SanitizerValidator {
  private readonly purify: ReturnType<typeof DOMPurify>;

  /**
   * Creates a new instance of SanitizerValidator.
   */
  constructor() {
    // Create a DOM window for server-side DOMPurify
    const window = new JSDOM("").window;
    this.purify = DOMPurify(window as unknown as typeof globalThis);
  }

  /**
   * Validates if the input string contains disallowed HTML or scripts.
   * @param input - The input string to validate.
   * @returns Object containing validation result and error message if invalid.
   */
  isValid(input: string): { valid: boolean; errorMessage: string } {
    if (!input) {
      return { valid: true, errorMessage: "" };
    }

    // Sanitize the input
    const sanitized = this.purify.sanitize(input);

    // If sanitized version differs from original, it contained malicious content
    if (input !== sanitized) {
      return {
        valid: false,
        errorMessage: "Input contains disallowed HTML or scripts.",
      };
    }

    return { valid: true, errorMessage: "" };
  }
}

// Export singleton instance
export const sanitizerValidator = new SanitizerValidator();
