import { IsNotEmpty, IsString, MaxLength } from "class-validator";
import { IsSanitized } from "../validators/sanitize.decorator";

/**
 * Request DTO for updating an order's message.
 */
export class UpdateOrderMessageRequest {
  /**
   * The new message for the order.
   */
  @IsNotEmpty()
  @IsString()
  @MaxLength(500)
  @IsSanitized()
  message: string;

  /**
   * Creates a new update order message request.
   * @param message - The new message for the order.
   */
  constructor(message: string) {
    this.message = message;
  }
}
