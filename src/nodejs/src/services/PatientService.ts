import "../mappers/PatientMapper";
import { ApiDataResult } from "../dtos/ApiResult";
import { BaseService } from "./BaseService";
import { CreatePatientRequest } from "../dtos/CreatePatientRequest";
import { IPatientRepository } from "../repositories/IPatientRepository";
import { IPatientService } from "./IPatientService";
import { Order } from "../entities/Order";
import { PagedRequest } from "../dtos/PagedRequest";
import { PagedResult } from "../dtos/PagedResult";
import { Patient } from "../entities/Patient";
import { PatientDto } from "../dtos/PatientDto";
import { toDtos } from "../mappers/PatientMapper";
import { DateUtils } from "../utils/dateUtils";

/**
 * Service for managing patient-related business logic.
 */
export class PatientService extends BaseService implements IPatientService {
  /**
   * Creates a new instance of PatientService.
   * @param patientRepository - The patient repository.
   */
  constructor(private patientRepository: IPatientRepository) {
    super();
  }

  /**
   * Retrieves a paginated list of patients within a date range.
   * @param request - The pagination request.
   * @param startTime - Optional start of the date range.
   * @param endTime - Optional end of the date range.
   * @returns An API result containing the paginated patient list.
   */
  async getPatientsAsync(
    request: PagedRequest,
    startTime?: Date,
    endTime?: Date
  ): Promise<ApiDataResult<PagedResult<PatientDto>>> {
    // Use UTC epoch as default start, current UTC time as default end
    const start = startTime || new Date(0);
    const end = endTime || DateUtils.utcNow();

    if (start > end) {
      return super.badRequestDefaultDataResult<PagedResult<PatientDto>>(
        "The start date must be earlier than or equal to the end date."
      );
    }

    const threeYearsLimit = 3 * 365 + 1;
    const durationMs = end.getTime() - start.getTime();
    const durationDays = Math.floor(durationMs / (1000 * 60 * 60 * 24));

    if (durationDays > threeYearsLimit) {
      return super.badRequestDefaultDataResult<PagedResult<PatientDto>>(
        `The date range must not exceed 3 years. Requested duration was ${durationDays} days.`
      );
    }

    const [patients, totalCount] = await this.patientRepository.getPatientsAsync(
      start,
      end,
      request.pageNumber,
      request.pageSize
    );

    if (totalCount === 0) {
      return super.noDataFoundPagedResult<PatientDto>("No patients found for the given date range.");
    }

    const patientDtos = toDtos(patients);
    return super.successPagedResult(patientDtos, totalCount, request.pageNumber, request.pageSize);
  }

  /**
   * Retrieves a patient by ID.
   * @param id - The patient ID.
   * @returns An API result containing the patient if found, null otherwise.
   */
  async getPatientAsync(id: number): Promise<ApiDataResult<PatientDto | null>> {
    const patient = await this.patientRepository.getPatientAsync(id);

    if (!patient) {
      return super.noDataFoundDataResult<PatientDto | null>(null, "Patient not found");
    }

    return super.successDataResult(patient.toDto());
  }

  /**
   * Creates a new patient.
   * @param request - The create patient request.
   * @returns An API result containing the created patient.
   */
  async createPatientAsync(request: CreatePatientRequest): Promise<ApiDataResult<PatientDto>> {
    const newOrder = new Order();
    newOrder.message = request.orderMessage;

    const newPatient = new Patient();
    newPatient.name = request.name;
    newPatient.orders = [newOrder];

    const savedPatient = await this.patientRepository.addAsync(newPatient);
    return super.successDataResult(savedPatient.toDto(), "Patient created");
  }
}
