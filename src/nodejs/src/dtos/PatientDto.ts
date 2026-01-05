import { OrderDto } from "./OrderDto";

/**
 * Data Transfer Object for patient information.
 */
export class PatientDto {
  /** The unique identifier of the patient. */
  id: number;
  /** The name of the patient. */
  name: string;
  /** The timestamp when the patient was created. */
  createdAt: Date;
  /** The timestamp when the patient was last updated. */
  updatedAt?: Date;
  /** The collection of orders associated with the patient. */
  orders: OrderDto[];

  /**
   * Creates a new patient DTO.
   * @param id - The unique identifier of the patient.
   * @param name - The name of the patient.
   * @param createdAt - The timestamp when the patient was created.
   * @param orders - The collection of orders associated with the patient.
   * @param updatedAt - The timestamp when the patient was last updated.
   */
  constructor(id: number, name: string, createdAt: Date, orders: OrderDto[] = [], updatedAt?: Date) {
    this.id = id;
    this.name = name;
    this.createdAt = createdAt;
    this.orders = orders;
    this.updatedAt = updatedAt;
  }
}
