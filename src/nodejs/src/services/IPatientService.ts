import { ApiDataResult } from "../dtos/ApiResult";
import { CreatePatientRequest } from "../dtos/CreatePatientRequest";
import { PagedRequest } from "../dtos/PagedRequest";
import { PagedResult } from "../dtos/PagedResult";
import { PatientDto } from "../dtos/PatientDto";

/**
 * Interface for patient service operations.
 */
export interface IPatientService {
  /**
   * Retrieves a paginated list of patients within a date range.
   * @param request - The pagination request.
   * @param startTime - Optional start of the date range.
   * @param endTime - Optional end of the date range.
   * @returns An API result containing the paginated patient list.
   */
  getPatientsAsync(
    request: PagedRequest,
    startTime?: Date,
    endTime?: Date
  ): Promise<ApiDataResult<PagedResult<PatientDto>>>;

  /**
   * Retrieves a patient by ID.
   * @param id - The patient ID.
   * @returns An API result containing the patient if found, null otherwise.
   */
  getPatientAsync(id: number): Promise<ApiDataResult<PatientDto | null>>;

  /**
   * Creates a new patient.
   * @param request - The create patient request.
   * @returns An API result containing the created patient.
   */
  createPatientAsync(request: CreatePatientRequest): Promise<ApiDataResult<PatientDto>>;
}
