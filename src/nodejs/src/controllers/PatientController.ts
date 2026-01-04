import { Request, Response } from "express";
import { IPatientService } from "../services/IPatientService";
import { PagedRequest } from "../dtos/PagedRequest";
import { CreatePatientRequest } from "../dtos/CreatePatientRequest";

export class PatientController {
  constructor(private patientService: IPatientService) {}

  getPatientsAsync = async (req: Request, res: Response) => {
    const pageNumber = parseInt(req.query.pageNumber as string) || 1;
    const pageSize = parseInt(req.query.pageSize as string) || 10;
    const startTime = req.query.startTime ? new Date(req.query.startTime as string) : undefined;
    const endTime = req.query.endTime ? new Date(req.query.endTime as string) : undefined;

    const request = new PagedRequest(pageNumber, pageSize);
    const result = await this.patientService.getPatientsAsync(request, startTime, endTime);
    
    res.status(200).json(result);
  };

  getPatientAsync = async (req: Request, res: Response) => {
    const patientId = parseInt(req.params.id);
    if (isNaN(patientId)) {
       res.status(400).json({ message: "Invalid patient ID" });
       return;
    }
    
    const result = await this.patientService.getPatientAsync(patientId);
    
    if (result.data) {
      res.status(200).json(result);
    } else {
      res.status(404).json(result);
    }
  }

  createPatientAsync = async (req: Request, res: Response) => {
    const { name, orderMessage } = req.body;
    if (!name || !orderMessage) {
        res.status(400).json({ message: "Name and OrderMessage are required" });
        return;
    }
    
    const request = new CreatePatientRequest(name, orderMessage);
    const result = await this.patientService.createPatientAsync(request);
    if (result.success) {
        res.status(201).json(result);
    } else {
        res.status(400).json(result);
    }
  }
}
