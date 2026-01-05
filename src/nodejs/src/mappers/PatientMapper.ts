import { Patient } from "../entities/Patient";
import { PatientDto } from "../dtos/PatientDto";
import { OrderDto } from "../dtos/OrderDto";

/**
 * Mapper for Patient related objects.
 */

// Extend the Patient interface to add the toDto method
declare module "../entities/Patient" {
  interface Patient {
    toDto(): PatientDto;
  }
}

/**
 * Maps a Patient entity to a PatientDto.
 * @param patient The patient entity.
 * @returns The patient DTO.
 */
Patient.prototype.toDto = function (): PatientDto {
  const ordersDto = this.orders?.map((o) => new OrderDto(o.id, o.message, o.patientId, o.createdAt, o.updatedAt)) || [];

  return new PatientDto(this.id, this.name, this.createdAt, ordersDto, this.updatedAt);
};

/**
 * Maps a collection of Patient entities to a collection of PatientDtos.
 * @param patients The collection of patient entities.
 * @returns The collection of patient DTOs.
 */
export function toDtos(patients: Patient[]): PatientDto[] {
  return patients.map((p) => p.toDto());
}
