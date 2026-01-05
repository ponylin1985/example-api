import { Patient } from "../entities/Patient";

/**
 * Interface for patient repository operations.
 */
export interface IPatientRepository {
  /**
   * Retrieves a paginated list of patients within a date range.
   * @param startTime - The start of the date range.
   * @param endTime - The end of the date range.
   * @param pageNumber - The page number to retrieve.
   * @param pageSize - The number of items per page.
   * @returns A tuple containing the patients array and total count.
   */
  getPatientsAsync(startTime: Date, endTime: Date, pageNumber: number, pageSize: number): Promise<[Patient[], number]>;

  /**
   * Checks if a patient exists by ID.
   * @param id - The patient ID.
   * @returns True if the patient exists, false otherwise.
   */
  isExistPatientAsync(id: number): Promise<boolean>;

  /**
   * Retrieves a patient by ID.
   * @param id - The patient ID.
   * @returns The patient if found, null otherwise.
   */
  getPatientAsync(id: number): Promise<Patient | null>;

  /**
   * Retrieves a patient by name.
   * @param name - The patient name.
   * @returns The patient if found, null otherwise.
   */
  getPatientByNameAsync(name: string): Promise<Patient | null>;

  /**
   * Adds a new patient to the repository.
   * @param patient - The patient to add.
   * @returns The added patient with generated ID.
   */
  addAsync(patient: Patient): Promise<Patient>;
}
