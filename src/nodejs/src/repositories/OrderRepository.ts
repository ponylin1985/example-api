import { AppDataSource } from "../data-source";
import { Order } from "../entities/Order";
import { IOrderRepository } from "./IOrderRepository";

export class OrderRepository implements IOrderRepository {
  private repository = AppDataSource.getRepository(Order);

  async getOrderAsync(id: number): Promise<Order | null> {
    return await this.repository.findOneBy({ id });
  }

  async addAsync(order: Order): Promise<Order> {
    return await this.repository.save(order);
  }

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
