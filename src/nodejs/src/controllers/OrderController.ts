import { CreateOrderRequest } from "../dtos/CreateOrderRequest";
import { IOrderService } from "../services/IOrderService";
import { Request, Response } from "express";

/**
 * Controller for handling order-related HTTP requests.
 */
export class OrderController {
  /**
   * Creates a new instance of OrderController.
   * @param orderService - The order service for business logic.
   */
  constructor(private orderService: IOrderService) {}

  /**
   * Retrieves a specific order by ID.
   * @param req - The Express request object.
   * @param res - The Express response object.
   */
  getOrderAsync = async (req: Request, res: Response) => {
    const id = parseInt(req.params.id);
    if (isNaN(id)) {
      res.status(400).json({ message: "Invalid order ID" });
      return;
    }

    const result = await this.orderService.getOrderAsync(id);

    if (result.success) {
      res.status(200).json(result);
    } else {
      res.status(404).json(result);
    }
  };

  /**
   * Creates a new order.
   * @param req - The Express request object.
   * @param res - The Express response object.
   */
  createOrderAsync = async (req: Request, res: Response) => {
    const { patientId, message } = req.body;
    if (!patientId || !message) {
      res.status(400).json({ message: "PatientId and Message are required" });
      return;
    }

    const request = new CreateOrderRequest(patientId, message);
    const result = await this.orderService.createOrderAsync(request);

    if (result.success) {
      res.status(201).json(result);
    } else {
      res.status(400).json(result);
    }
  };

  /**
   * Updates the message of an existing order.
   * @param req - The Express request object.
   * @param res - The Express response object.
   */
  updateOrderMessageAsync = async (req: Request, res: Response) => {
    const id = parseInt(req.params.id);
    if (isNaN(id)) {
      res.status(400).json({ message: "Invalid order ID" });
      return;
    }

    const { message } = req.body;
    if (!message) {
      res.status(400).json({ message: "Message is required" });
      return;
    }

    const result = await this.orderService.updateMessageAsync(id, message);

    if (result.success) {
      res.status(200).json(result);
    } else {
      res.status(404).json(result);
    }
  };
}
