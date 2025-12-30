import { Router } from "express";
import { PatientController } from "../controllers/patient.controller";

const router = Router();

/**
 * @swagger
 * /api/patients:
 *   get:
 *     summary: Retrieve a list of patients
 *     tags: [Patients]
 *     responses:
 *       200:
 *         description: A list of patients
 */
router.get("/", PatientController.getPatientsAsync);

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
 *           type: string
 *         description: The patient ID
 *     responses:
 *       200:
 *         description: A single patient
 */
router.get("/:id", PatientController.getPatientAsync);

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
 *               age:
 *                 type: integer
 *             required:
 *               - name
 *               - age
 *     responses:
 *       201:
 *         description: Patient created successfully
 */
router.post("/", PatientController.createPatientAsync);

export default router;
