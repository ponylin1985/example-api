import { ApiDataResult } from "../dtos/ApiResult";
import { OrderDto } from "../dtos/OrderDto";
import { CreateOrderRequest } from "../dtos/CreateOrderRequest";

export interface IOrderService {
  getOrderAsync(id: number): Promise<ApiDataResult<OrderDto | null>>;
  createOrderAsync(request: CreateOrderRequest): Promise<ApiDataResult<OrderDto>>;
  updateMessageAsync(id: number, message: string): Promise<ApiDataResult<OrderDto>>;
}
