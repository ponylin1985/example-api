import { Router } from "express";
import { OrderController } from "../controllers/OrderController";
import { OrderService } from "../services/OrderService";
import { OrderRepository } from "../repositories/OrderRepository";
import { PatientRepository } from "../repositories/PatientRepository";

const router = Router();

const orderRepository = new OrderRepository();
const patientRepository = new PatientRepository();
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
router.get("/:id", orderController.getOrderAsync);

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
router.post("/", orderController.createOrderAsync);

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
router.put("/:id", orderController.updateOrderMessageAsync);

export default router;
