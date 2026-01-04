import { AppDataSource } from "../data-source";
import { Patient } from "../entities/Patient";
import { IPatientRepository } from "./IPatientRepository";
import { Between } from "typeorm";

export class PatientRepository implements IPatientRepository {
  private repository = AppDataSource.getRepository(Patient);

  async getPatientsAsync(startTime: Date, endTime: Date, pageNumber: number, pageSize: number): Promise<[Patient[], number]> {
    return await this.repository.findAndCount({
      where: {
        createdAt: Between(startTime, endTime)
      },
      relations: {
        orders: true
      },
      order: {
        id: "DESC",
        orders: {
          id: "DESC"
        }
      },
      skip: (pageNumber - 1) * pageSize,
      take: pageSize
    });
  }

  async isExistPatientAsync(id: number): Promise<boolean> {
    return await this.repository.existsBy({ id });
  }

  async getPatientAsync(id: number): Promise<Patient | null> {
    return await this.repository.findOne({
      where: { id },
      relations: { orders: true },
      order: {
        orders: {
          id: "DESC"
        }
      }
    });
  }

  async getPatientByNameAsync(name: string): Promise<Patient | null> {
    return await this.repository.findOne({
      where: { name },
      relations: { orders: true },
      order: {
        orders: {
          id: "DESC"
        }
      }
    });
  }

  async addAsync(patient: Patient): Promise<Patient> {
    return await this.repository.save(patient);
  }
}
