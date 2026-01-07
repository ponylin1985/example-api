"""Patient service."""

import math
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.orm import selectinload

from app.entities import Patient, Order
from app.repositories import PatientRepository
from app.schemas import PatientDto, GetPatientsRequest, CreatePatientRequest, ApiResultData, PagedResult, ApiCode


class PatientService:
    """Service for patient business logic."""

    def __init__(self, db: AsyncSession):
        """Initialize service with database session."""
        self.db = db
        self.patient_repo = PatientRepository(db)

    async def get_patients(self, request: GetPatientsRequest) -> ApiResultData[PagedResult[PatientDto]]:
        """
        Get patients within date range with pagination.

        Args:
            request: Request with date range and pagination parameters

        Returns:
            API result with paged patient data
        """
        try:
            patients, total_count = await self.patient_repo.get_patients(
                start_time=request.start_time,
                end_time=request.end_time,
                page_number=request.page_number,
                page_size=request.page_size,
            )

            total_pages = math.ceil(total_count / request.page_size) if total_count > 0 else 1
            patient_dtos = [PatientDto.model_validate(patient) for patient in patients]
            paged_result = PagedResult[PatientDto](
                data=patient_dtos,
                page_number=request.page_number,
                total_pages=total_pages,
                total_count=total_count,
                page_size=request.page_size,
            )

            return ApiResultData[PagedResult[PatientDto]](
                success=True, code=ApiCode.SUCCESS, message="Patients retrieved successfully", data=paged_result
            )

        except Exception as e:
            return ApiResultData[PagedResult[PatientDto]](
                success=False,
                code=ApiCode.DATA_ACCESS_ERROR,
                message=f"Failed to retrieve patients: {str(e)}",
                data=None,
            )

    async def get_patient(self, patient_id: int) -> ApiResultData[PatientDto]:
        """
        Get patient by ID.

        Args:
            patient_id: Patient ID

        Returns:
            API result with patient data
        """
        try:
            patient = await self.patient_repo.get_patient(patient_id)

            if not patient:
                return ApiResultData[PatientDto](
                    success=False,
                    code=ApiCode.NO_DATA_FOUND,
                    message=f"Patient with ID {patient_id} not found",
                    data=None,
                )

            patient_dto = PatientDto.model_validate(patient)
            return ApiResultData[PatientDto](
                success=True, code=ApiCode.SUCCESS, message="Patient retrieved successfully", data=patient_dto
            )

        except Exception as e:
            return ApiResultData[PatientDto](
                success=False,
                code=ApiCode.DATA_ACCESS_ERROR,
                message=f"Failed to retrieve patient: {str(e)}",
                data=None,
            )

    async def create_patient(self, request: CreatePatientRequest) -> ApiResultData[PatientDto]:
        """
        Create a new patient with an initial order.

        Args:
            request: Request with patient and order data

        Returns:
            API result with created patient data
        """
        try:
            patient = Patient(name=request.name)
            order = Order(message=request.order_message)
            patient.orders.append(order)

            created_patient = await self.patient_repo.add(patient)
            await self.db.commit()
            await self.db.refresh(created_patient, ["orders"])

            patient_dto = PatientDto.model_validate(created_patient)
            return ApiResultData[PatientDto](
                success=True, code=ApiCode.SUCCESS, message="Patient created successfully", data=patient_dto
            )

        except Exception as e:
            await self.db.rollback()
            return ApiResultData[PatientDto](
                success=False, code=ApiCode.OPERATION_FAILED, message=f"Failed to create patient: {str(e)}", data=None
            )
