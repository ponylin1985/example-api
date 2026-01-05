import { IsNotEmpty, IsNumber, IsString, MaxLength, Min, Max } from "class-validator";
import { IsSanitized } from "../validators/sanitize.decorator";

/**
 * Request DTO for creating a new order.
 */
export class CreateOrderRequest {
  /**
   * The ID of the patient who owns the order.
   */
  @IsNumber()
  @Min(1, { message: "PatientId must be greater than 0." })
  @Max(Number.MAX_SAFE_INTEGER, { message: "PatientId exceeds maximum safe integer." })
  patientId: number;

  /**
   * The message associated with the order.
   */
  @IsNotEmpty()
  @IsString()
  @MaxLength(500)
  @IsSanitized()
  message: string;

  /**
   * Creates a new create order request.
   * @param patientId - The ID of the patient who owns the order.
   * @param message - The message associated with the order.
   */
  constructor(patientId: number, message: string) {
    this.patientId = patientId;
    this.message = message;
  }
}
