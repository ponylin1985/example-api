import {
  registerDecorator,
  ValidationOptions,
  ValidatorConstraint,
  ValidatorConstraintInterface,
} from "class-validator";
import { sanitizerValidator } from "./sanitizerValidator";

/**
 * Custom validator constraint for XSS sanitization.
 */
@ValidatorConstraint({ name: "isSanitized", async: false })
export class IsSanitizedConstraint implements ValidatorConstraintInterface {
  /**
   * Validates that the value doesn't contain malicious HTML or scripts.
   * @param value - The value to validate.
   * @returns True if valid, false otherwise.
   */
  validate(value: string): boolean {
    if (typeof value !== "string") {
      return true; // Let other validators handle type checking
    }

    const result = sanitizerValidator.isValid(value);
    return result.valid;
  }

  /**
   * Default error message when validation fails.
   * @returns The error message.
   */
  defaultMessage(): string {
    return "Input contains disallowed HTML or scripts.";
  }
}

/**
 * Decorator to validate that a string property doesn't contain XSS attacks.
 * @param validationOptions - Optional validation options.
 * @returns PropertyDecorator
 *
 * @example
 * class CreateUserDto {
 *   @IsSanitized()
 *   @IsString()
 *   name: string;
 * }
 */
export function IsSanitized(validationOptions?: ValidationOptions) {
  return function (object: object, propertyName: string) {
    registerDecorator({
      target: object.constructor,
      propertyName: propertyName,
      options: validationOptions,
      constraints: [],
      validator: IsSanitizedConstraint,
    });
  };
}
