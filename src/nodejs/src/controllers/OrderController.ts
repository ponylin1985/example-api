import { Request, Response } from "express";
import { IOrderService } from "../services/IOrderService";
import { CreateOrderRequest } from "../dtos/CreateOrderRequest";
import { UpdateOrderMessageRequest } from "../dtos/UpdateOrderMessageRequest";

export class OrderController {
  constructor(private orderService: IOrderService) {}

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
  }

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
  }

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
  }
}
