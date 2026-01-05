import { AppDataSource } from "../data-source";
import { Between } from "typeorm";
import { IPatientRepository } from "./IPatientRepository";
import { Patient } from "../entities/Patient";

/**
 * Repository implementation for patient data access.
 */
export class PatientRepository implements IPatientRepository {
  private repository = AppDataSource.getRepository(Patient);

  /**
   * Retrieves a paginated list of patients within a date range.
   * @param startTime - The start of the date range.
   * @param endTime - The end of the date range.
   * @param pageNumber - The page number to retrieve.
   * @param pageSize - The number of items per page.
   * @returns A tuple containing the patients array and total count.
   */
  async getPatientsAsync(
    startTime: Date,
    endTime: Date,
    pageNumber: number,
    pageSize: number
  ): Promise<[Patient[], number]> {
    return await this.repository.findAndCount({
      where: {
        createdAt: Between(startTime, endTime),
      },
      relations: {
        orders: true,
      },
      order: {
        id: "DESC",
        orders: {
          id: "DESC",
        },
      },
      skip: (pageNumber - 1) * pageSize,
      take: pageSize,
    });
  }

  /**
   * Checks if a patient exists by ID.
   * @param id - The patient ID.
   * @returns True if the patient exists, false otherwise.
   */
  async isExistPatientAsync(id: number): Promise<boolean> {
    return await this.repository.existsBy({ id });
  }

  /**
   * Retrieves a patient by ID.
   * @param id - The patient ID.
   * @returns The patient if found, null otherwise.
   */
  async getPatientAsync(id: number): Promise<Patient | null> {
    return await this.repository.findOne({
      where: { id },
      relations: { orders: true },
      order: {
        orders: {
          id: "DESC",
        },
      },
    });
  }

  /**
   * Retrieves a patient by name.
   * @param name - The patient name.
   * @returns The patient if found, null otherwise.
   */
  async getPatientByNameAsync(name: string): Promise<Patient | null> {
    return await this.repository.findOne({
      where: { name },
      relations: { orders: true },
      order: {
        orders: {
          id: "DESC",
        },
      },
    });
  }

  /**
   * Adds a new patient to the repository.
   * @param patient - The patient to add.
   * @returns The added patient with generated ID.
   */
  async addAsync(patient: Patient): Promise<Patient> {
    return await this.repository.save(patient);
  }
}
