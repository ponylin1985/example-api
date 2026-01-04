import { IOrderRepository } from "../repositories/IOrderRepository";
import { IPatientRepository } from "../repositories/IPatientRepository";
import { IOrderService } from "./IOrderService";
import { ApiDataResult } from "../dtos/ApiResult";
import { OrderDto } from "../dtos/OrderDto";
import { ApiCode } from "../dtos/ApiCode";
import { CreateOrderRequest } from "../dtos/CreateOrderRequest";
import { Order } from "../entities/Order";

export class OrderService implements IOrderService {
  constructor(
    private orderRepository: IOrderRepository,
    private patientRepository: IPatientRepository
  ) {}

  async getOrderAsync(id: number): Promise<ApiDataResult<OrderDto | null>> {
    const order = await this.orderRepository.getOrderAsync(id);

    if (!order) {
      return new ApiDataResult<OrderDto | null>(true, ApiCode.NoDataFound, "Order not found", null);
    }

    return new ApiDataResult<OrderDto>(true, ApiCode.Success, "Success", this.mapToDto(order));
  }

  async createOrderAsync(request: CreateOrderRequest): Promise<ApiDataResult<OrderDto>> {
    const patientExists = await this.patientRepository.isExistPatientAsync(request.patientId);

    if (!patientExists) {
      return new ApiDataResult<OrderDto>(false, ApiCode.OperationFailed, `Order with PatientId ${request.patientId} not found.`);
    }

    const order = new Order();
    order.patientId = request.patientId;
    order.message = request.message;
    
    // In C#, CreatedAt is set by DB default or manually? 
    // Entity definition has @CreateDateColumn so TypeORM handles it.
    const createdOrder = await this.orderRepository.addAsync(order);
    
    return new ApiDataResult<OrderDto>(true, ApiCode.Success, "Success", this.mapToDto(createdOrder));
  }

  async updateMessageAsync(id: number, message: string): Promise<ApiDataResult<OrderDto>> {
    const updatedAt = new Date();
    const updatedOrder = await this.orderRepository.updateAsync(id, message, updatedAt);

    if (!updatedOrder) {
      return new ApiDataResult<OrderDto>(false, ApiCode.OperationFailed, `Order with ID ${id} not found.`);
    }

    return new ApiDataResult<OrderDto>(true, ApiCode.Success, "Success", this.mapToDto(updatedOrder));
  }

  private mapToDto(order: Order): OrderDto {
    return new OrderDto(order.id, order.message, order.patientId, order.createdAt, order.updatedAt);
  }
}
