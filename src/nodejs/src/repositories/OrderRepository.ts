import { AppDataSource } from "../data-source";
import { IOrderRepository } from "./IOrderRepository";
import { Order } from "../entities/Order";

/**
 * Repository implementation for order data access.
 */
export class OrderRepository implements IOrderRepository {
  private repository = AppDataSource.getRepository(Order);

  /**
   * Retrieves an order by its ID.
   * @param id - The order ID.
   * @returns The order if found, null otherwise.
   */
  async getOrderAsync(id: number): Promise<Order | null> {
    return await this.repository.findOneBy({ id });
  }

  /**
   * Adds a new order to the repository.
   * @param order - The order to add.
   * @returns The added order with generated ID.
   */
  async addAsync(order: Order): Promise<Order> {
    return await this.repository.save(order);
  }

  /**
   * Updates an order's message.
   * @param id - The order ID.
   * @param message - The new message.
   * @param updatedAt - The update timestamp.
   * @returns The updated order if found, null otherwise.
   */
  async updateAsync(id: number, message: string, updatedAt: Date): Promise<Order | null> {
    const order = await this.repository.findOneBy({ id });
    if (!order) {
      return null;
    }
    order.message = message;
    order.updatedAt = updatedAt;
    return await this.repository.save(order);
  }
}
