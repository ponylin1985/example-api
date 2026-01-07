"""Patient API endpoints."""

import logging
from fastapi import APIRouter, Depends, Query
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import get_db
from app.services import PatientService
from app.schemas import GetPatientsRequest, CreatePatientRequest, PatientDto, ApiResultData, PagedResult

router = APIRouter(prefix="/api/patients", tags=["Patients"])
logger = logging.getLogger(__name__)


@router.get("/", response_model=ApiResultData[PagedResult[PatientDto]])
async def get_patients(
    start_time: str = Query(..., alias="startTime", description="Start time of the date range (UTC)"),
    end_time: str = Query(..., alias="endTime", description="End time of the date range (UTC)"),
    page_number: int = Query(1, alias="pageNumber", ge=1, description="Page number to retrieve"),
    page_size: int = Query(10, alias="pageSize", ge=1, le=100, description="Number of items per page"),
    db: AsyncSession = Depends(get_db),
):
    """
    Get patients created within a specified time range.
    """
    logger.info(
        "Received request to get patients with parameters: startTime=%s, endTime=%s, pageNumber=%s, pageSize=%s",
        start_time,
        end_time,
        page_number,
        page_size,
    )

    try:
        # Parse datetime strings
        from datetime import datetime

        request = GetPatientsRequest(
            start_time=datetime.fromisoformat(start_time.replace("Z", "+00:00")),
            end_time=datetime.fromisoformat(end_time.replace("Z", "+00:00")),
            page_number=page_number,
            page_size=page_size,
        )
    except ValueError as e:
        from app.schemas import ApiCode

        logger.warning("Invalid datetime format: %s", str(e))
        return ApiResultData[PagedResult[PatientDto]](
            success=False, code=ApiCode.INVALID_REQUEST, message=f"Invalid datetime format: {str(e)}", data=None
        )

    service = PatientService(db)
    result = await service.get_patients(request)

    if not result.success:
        logger.warning("Failed to retrieve patients: %s", result.message)
    else:
        logger.info("Successfully retrieved %s patients.", result.data.total_count if result.data else 0)

    return result


@router.get("/{patient_id}", response_model=ApiResultData[PatientDto])
async def get_patient_by_id(patient_id: int, db: AsyncSession = Depends(get_db)):
    """
    Get a patient by their ID.
    """
    logger.info("Received request to get patient with ID: %s", patient_id)

    service = PatientService(db)
    result = await service.get_patient(patient_id)

    if not result.success:
        logger.warning("Failed to retrieve patient with ID %s: %s", patient_id, result.message)
    else:
        logger.info("Successfully retrieved patient with ID: %s", patient_id)

    return result


@router.post("/", response_model=ApiResultData[PatientDto])
async def create_patient(request: CreatePatientRequest, db: AsyncSession = Depends(get_db)):
    """
    Create a new patient record.
    """
    logger.info("Received request to create a new patient: name=%s", request.name)

    service = PatientService(db)
    result = await service.create_patient(request)

    if not result.success:
        logger.warning("Failed to create patient: %s", result.message)
    else:
        logger.info("Successfully created patient with ID: %s", result.data.id if result.data else "N/A")

    return result
