import { Request, Response } from "express";

export class PatientController {
  static getPatientsAsync = async (req: Request, res: Response) => {
    res.status(200).json({ message: "Get all patients" });
  };

  static getPatientAsync = async (req: Request, res: Response) => {
    const patientId = req.params.id;
    res.status(200).json({ message: `Get patient with ID: ${patientId}` });
  }

  static createPatientAsync = async (req: Request, res: Response) => {
    const patientData = req.body;
    res.status(201).json({ message: "Patient created", data: patientData });
  }
}
