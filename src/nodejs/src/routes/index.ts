import { Router } from "express";
import patientRoutes from "./patient.routes";

const router = Router();
router.use("/patients", patientRoutes);

export default router;
