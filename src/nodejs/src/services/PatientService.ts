import { Patient } from "../entities/Patient";
import { IPatientRepository } from "../repositories/IPatientRepository";
import { IPatientService } from "./IPatientService";
import { ApiDataResult } from "../dtos/ApiResult";
import { PatientDto } from "../dtos/PatientDto";
import { ApiCode } from "../dtos/ApiCode";
import { OrderDto } from "../dtos/OrderDto";
import { PagedRequest } from "../dtos/PagedRequest";
import { PagedResult } from "../dtos/PagedResult";
import { CreatePatientRequest } from "../dtos/CreatePatientRequest";
import { Order } from "../entities/Order";

export class PatientService implements IPatientService {
  constructor(private patientRepository: IPatientRepository) {}

  async getPatientsAsync(request: PagedRequest, startTime?: Date, endTime?: Date): Promise<ApiDataResult<PagedResult<PatientDto>>> {
    const start = startTime || new Date(0);
    const end = endTime || new Date();

    const [patients, totalCount] = await this.patientRepository.getPatientsAsync(start, end, request.pageNumber, request.pageSize);

    const patientDtos = patients.map(p => this.mapToDto(p));
    const pagedResult = new PagedResult(patientDtos, totalCount, request.pageNumber, request.pageSize);

    return new ApiDataResult<PagedResult<PatientDto>>(true, ApiCode.Success, "Success", pagedResult);
  }
  
  async getPatientAsync(id: number): Promise<ApiDataResult<PatientDto | null>> {
    const patient = await this.patientRepository.getPatientAsync(id);

    if (!patient) {
      return new ApiDataResult<PatientDto | null>(true, ApiCode.NoDataFound, "Patient not found", null);
    }

    return new ApiDataResult<PatientDto>(true, ApiCode.Success, "Success", this.mapToDto(patient));
  }

  async createPatientAsync(request: CreatePatientRequest): Promise<ApiDataResult<PatientDto>> {
    const newOrder = new Order();
    newOrder.message = request.orderMessage;
    
    const newPatient = new Patient();
    newPatient.name = request.name;
    newPatient.orders = [newOrder];
    
    const savedPatient = await this.patientRepository.addAsync(newPatient);
    return new ApiDataResult<PatientDto>(true, ApiCode.Success, "Patient created", this.mapToDto(savedPatient));
  }

  private mapToDto(patient: Patient): PatientDto {
    const ordersDto = patient.orders?.map(o => new OrderDto(o.id, o.message, o.patientId, o.createdAt, o.updatedAt)) || [];
    return new PatientDto(patient.id, patient.name, patient.createdAt, ordersDto, patient.updatedAt);
  }
}
