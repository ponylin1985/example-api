import { PatientDto } from "../dtos/PatientDto";
import { ApiDataResult } from "../dtos/ApiResult";
import { PagedResult } from "../dtos/PagedResult";
import { PagedRequest } from "../dtos/PagedRequest";
import { CreatePatientRequest } from "../dtos/CreatePatientRequest";

export interface IPatientService {
  getPatientsAsync(request: PagedRequest, startTime?: Date, endTime?: Date): Promise<ApiDataResult<PagedResult<PatientDto>>>;
  getPatientAsync(id: number): Promise<ApiDataResult<PatientDto | null>>;
  createPatientAsync(request: CreatePatientRequest): Promise<ApiDataResult<PatientDto>>;
}
