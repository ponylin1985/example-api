import { Router } from "express";
import orderRoutes from "./order.routes";
import patientRoutes from "./patient.routes";

const router = Router();
router.use("/patients", patientRoutes);
router.use("/orders", orderRoutes);

export default router;
