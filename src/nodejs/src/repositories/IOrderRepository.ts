import { Order } from "../entities/Order";

/**
 * Interface for order repository operations.
 */
export interface IOrderRepository {
  /**
   * Retrieves an order by its ID.
   * @param id - The order ID.
   * @returns The order if found, null otherwise.
   */
  getOrderAsync(id: number): Promise<Order | null>;

  /**
   * Adds a new order to the repository.
   * @param order - The order to add.
   * @returns The added order with generated ID.
   */
  addAsync(order: Order): Promise<Order>;

  /**
   * Updates an order's message.
   * @param id - The order ID.
   * @param message - The new message.
   * @param updatedAt - The update timestamp.
   * @returns The updated order if found, null otherwise.
   */
  updateAsync(id: number, message: string, updatedAt: Date): Promise<Order | null>;
}
