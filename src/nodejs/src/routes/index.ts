import { Router } from "express";
import patientRoutes from "./patient.routes";
import orderRoutes from "./order.routes";

const router = Router();
router.use("/patients", patientRoutes);
router.use("/orders", orderRoutes);

export default router;
