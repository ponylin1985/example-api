import { Patient } from "../entities/Patient";

export interface IPatientRepository {
  getPatientsAsync(startTime: Date, endTime: Date, pageNumber: number, pageSize: number): Promise<[Patient[], number]>;
  isExistPatientAsync(id: number): Promise<boolean>;
  getPatientAsync(id: number): Promise<Patient | null>;
  getPatientByNameAsync(name: string): Promise<Patient | null>;
  addAsync(patient: Patient): Promise<Patient>;
}
