import { CachedPatientRepository } from "../repositories/CachedPatientRepository";
import { CreatePatientRequest } from "../dtos/CreatePatientRequest";
import { PatientController } from "../controllers/PatientController";
import { PatientRepository } from "../repositories/PatientRepository";
import { PatientService } from "../services/PatientService";
import { Router } from "express";
import { validateBody } from "../middlewares/validation";
import { validateId } from "../middlewares/validateId";

const router = Router();
const patientRepository = new CachedPatientRepository(new PatientRepository());
const patientService = new PatientService(patientRepository);
const patientController = new PatientController(patientService);

/**
 * @swagger
 * /api/patients:
 *   get:
 *     summary: Retrieve a list of patients
 *     tags: [Patients]
 *     parameters:
 *       - in: query
 *         name: pageNumber
 *         schema:
 *           type: integer
 *           default: 1
 *         description: Page number
 *       - in: query
 *         name: pageSize
 *         schema:
 *           type: integer
 *           default: 10
 *         description: Number of items per page
 *       - in: query
 *         name: startTime
 *         schema:
 *           type: string
 *           format: date-time
 *         description: Filter by creation time (start)
 *       - in: query
 *         name: endTime
 *         schema:
 *           type: string
 *           format: date-time
 *         description: Filter by creation time (end)
 *     responses:
 *       200:
 *         description: A list of patients
 */
router.get("/", patientController.getPatientsAsync);

/**
 * @swagger
 * /api/patients/{id}:
 *   get:
 *     summary: Retrieve a single patient by ID
 *     tags: [Patients]
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: integer
 *         description: The patient ID
 *     responses:
 *       200:
 *         description: A single patient
 *       404:
 *         description: Patient not found
 */
router.get("/:id", validateId, patientController.getPatientAsync);

/**
 * @swagger
 * /api/patients:
 *   post:
 *     summary: Create a new patient
 *     tags: [Patients]
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               name:
 *                 type: string
 *               orderMessage:
 *                 type: string
 *             required:
 *               - name
 *               - orderMessage
 *     responses:
 *       201:
 *         description: Patient created successfully
 *       400:
 *         description: Invalid request
 */
router.post("/", validateBody(CreatePatientRequest), patientController.createPatientAsync);
export default router;
