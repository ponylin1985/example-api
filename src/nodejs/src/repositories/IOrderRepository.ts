import { Order } from "../entities/Order";

export interface IOrderRepository {
  getOrderAsync(id: number): Promise<Order | null>;
  addAsync(order: Order): Promise<Order>;
  updateAsync(id: number, message: string, updatedAt: Date): Promise<Order | null>;
}
