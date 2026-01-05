import { IsNotEmpty, IsString, MaxLength } from "class-validator";
import { IsSanitized } from "../validators/sanitize.decorator";

/**
 * Request DTO for creating a new patient.
 */
export class CreatePatientRequest {
  /**
   * The name of the patient.
   */
  @IsNotEmpty()
  @IsString()
  @MaxLength(50)
  @IsSanitized()
  name: string;

  /**
   * The initial order message for the patient.
   */
  @IsNotEmpty()
  @IsString()
  @MaxLength(500)
  @IsSanitized()
  orderMessage: string;

  /**
   * Creates a new create patient request.
   * @param name - The name of the patient.
   * @param orderMessage - The initial order message for the patient.
   */
  constructor(name: string, orderMessage: string) {
    this.name = name;
    this.orderMessage = orderMessage;
  }
}
