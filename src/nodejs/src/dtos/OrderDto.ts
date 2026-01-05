/**
 * Data Transfer Object for order information.
 */
export class OrderDto {
  /** The unique identifier of the order. */
  id: number;
  /** The message associated with the order. */
  message: string;
  /** The ID of the patient who owns the order. */
  patientId: number;
  /** The timestamp when the order was created. */
  createdAt: Date;
  /** The timestamp when the order was last updated. */
  updatedAt?: Date;

  /**
   * Creates a new order DTO.
   * @param id - The unique identifier of the order.
   * @param message - The message associated with the order.
   * @param patientId - The ID of the patient who owns the order.
   * @param createdAt - The timestamp when the order was created.
   * @param updatedAt - The timestamp when the order was last updated.
   */
  constructor(id: number, message: string, patientId: number, createdAt: Date, updatedAt?: Date) {
    this.id = id;
    this.message = message;
    this.patientId = patientId;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
  }
}
