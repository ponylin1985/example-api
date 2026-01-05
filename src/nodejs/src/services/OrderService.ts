import "../mappers/OrderMapper";
import { ApiCode } from "../dtos/ApiCode";
import { ApiDataResult } from "../dtos/ApiResult";
import { BaseService } from "./BaseService";
import { CreateOrderRequest } from "../dtos/CreateOrderRequest";
import { DateUtils } from "../utils/DateUtils";
import { IOrderRepository } from "../repositories/IOrderRepository";
import { IOrderService } from "./IOrderService";
import { IPatientRepository } from "../repositories/IPatientRepository";
import { Order } from "../entities/Order";
import { OrderDto } from "../dtos/OrderDto";

/**
 * Service for managing order-related business logic.
 */
export class OrderService extends BaseService implements IOrderService {
  /**
   * Creates a new instance of OrderService.
   * @param orderRepository - The order repository.
   * @param patientRepository - The patient repository.
   */
  constructor(
    private orderRepository: IOrderRepository,
    private patientRepository: IPatientRepository
  ) {
    super();
  }

  /**
   * Retrieves an order by its ID.
   * @param id - The order ID.
   * @returns An API result containing the order if found, null otherwise.
   */
  async getOrderAsync(id: number): Promise<ApiDataResult<OrderDto | null>> {
    const order = await this.orderRepository.getOrderAsync(id);

    if (!order) {
      return this.noDataFoundDataResult<OrderDto | null>(null, "Order not found");
    }

    return this.successDataResult(order.toDto());
  }

  /**
   * Creates a new order.
   * @param request - The create order request.
   * @returns An API result containing the created order.
   */
  async createOrderAsync(request: CreateOrderRequest): Promise<ApiDataResult<OrderDto>> {
    const patientExists = await this.patientRepository.isExistPatientAsync(request.patientId);

    if (!patientExists) {
      return super.failureDataResult<OrderDto>(
        ApiCode.OperationFailed,
        undefined,
        `Order with PatientId ${request.patientId} not found.`
      );
    }

    const order = new Order();
    order.patientId = request.patientId;
    order.message = request.message.trim();

    const createdOrder = await this.orderRepository.addAsync(order);
    return super.successDataResult(createdOrder.toDto());
  }

  /**
   * Updates an order's message.
   * @param id - The order ID.
   * @param message - The new message.
   * @returns An API result containing the updated order.
   */
  async updateMessageAsync(id: number, message: string): Promise<ApiDataResult<OrderDto>> {
    const updatedAt = DateUtils.utcNow();
    const updatedOrder = await this.orderRepository.updateAsync(id, message.trim(), updatedAt);

    if (!updatedOrder) {
      return super.failureDataResult<OrderDto>(ApiCode.OperationFailed, undefined, `Order with ID ${id} not found.`);
    }

    return super.successDataResult(updatedOrder.toDto());
  }
}
