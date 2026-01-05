import { Order } from "../entities/Order";
import { OrderDto } from "../dtos/OrderDto";

/**
 * Mapper for Order related objects.
 */

// Extend the Order interface to add the toDto method
declare module "../entities/Order" {
  interface Order {
    toDto(): OrderDto;
  }
}

/**
 * Maps an Order entity to an OrderDto.
 * @param order The order entity.
 * @returns The order DTO.
 */
Order.prototype.toDto = function (): OrderDto {
  return new OrderDto(this.id, this.message, this.patientId, this.createdAt, this.updatedAt);
};

/**
 * Maps a collection of Order entities to a collection of OrderDtos.
 * @param orders The collection of order entities.
 * @returns The collection of order DTOs.
 */
export function toDtos(orders: Order[]): OrderDto[] {
  return orders.map((o) => o.toDto());
}
