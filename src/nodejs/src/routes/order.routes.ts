import { CachedOrderRepository } from "../repositories/CachedOrderRepository";
import { CachedPatientRepository } from "../repositories/CachedPatientRepository";
import { CreateOrderRequest } from "../dtos/CreateOrderRequest";
import { OrderController } from "../controllers/OrderController";
import { OrderRepository } from "../repositories/OrderRepository";
import { OrderService } from "../services/OrderService";
import { PatientRepository } from "../repositories/PatientRepository";
import { Router } from "express";
import { UpdateOrderMessageRequest } from "../dtos/UpdateOrderMessageRequest";
import { validateBody } from "../middlewares/validation";
import { validateId } from "../middlewares/validateId";

const router = Router();
const orderRepository = new CachedOrderRepository(new OrderRepository());
const patientRepository = new CachedPatientRepository(new PatientRepository());
const orderService = new OrderService(orderRepository, patientRepository);
const orderController = new OrderController(orderService);

/**
 * @swagger
 * /api/orders/{id}:
 *   get:
 *     summary: Get an order by its identifier.
 *     tags: [Orders]
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: integer
 *         description: The order ID
 *     responses:
 *       200:
 *         description: The order DTO
 *       404:
 *         description: Order not found
 */
router.get("/:id", validateId, orderController.getOrderAsync);

/**
 * @swagger
 * /api/orders:
 *   post:
 *     summary: Create a new order.
 *     tags: [Orders]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               patientId:
 *                 type: integer
 *               message:
 *                 type: string
 *             required:
 *               - patientId
 *               - message
 *     responses:
 *       201:
 *         description: The created order DTO
 *       400:
 *         description: Invalid request
 */
router.post("/", validateBody(CreateOrderRequest), orderController.createOrderAsync);

/**
 * @swagger
 * /api/orders/{id}:
 *   put:
 *     summary: Update the message of an existing order.
 *     tags: [Orders]
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: integer
 *         description: The order ID
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               message:
 *                 type: string
 *             required:
 *               - message
 *     responses:
 *       200:
 *         description: The updated order DTO
 *       404:
 *         description: Order not found
 */
router.put("/:id", validateId, validateBody(UpdateOrderMessageRequest), orderController.updateOrderMessageAsync);
export default router;
