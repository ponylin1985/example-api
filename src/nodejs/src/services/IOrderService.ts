import { ApiDataResult } from "../dtos/ApiResult";
import { CreateOrderRequest } from "../dtos/CreateOrderRequest";
import { OrderDto } from "../dtos/OrderDto";

/**
 * Interface for order service operations.
 */
export interface IOrderService {
  /**
   * Retrieves an order by its ID.
   * @param id - The order ID.
   * @returns An API result containing the order if found, null otherwise.
   */
  getOrderAsync(id: number): Promise<ApiDataResult<OrderDto | null>>;

  /**
   * Creates a new order.
   * @param request - The create order request.
   * @returns An API result containing the created order.
   */
  createOrderAsync(request: CreateOrderRequest): Promise<ApiDataResult<OrderDto>>;

  /**
   * Updates an order's message.
   * @param id - The order ID.
   * @param message - The new message.
   * @returns An API result containing the updated order.
   */
  updateMessageAsync(id: number, message: string): Promise<ApiDataResult<OrderDto>>;
}
