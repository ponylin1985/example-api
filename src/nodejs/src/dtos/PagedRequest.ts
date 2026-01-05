import { IsNumber, Max, Min } from "class-validator";

/**
 * Request DTO for paginated queries.
 */
export class PagedRequest {
  /** The page number to retrieve (1-indexed). */
  @IsNumber()
  @Min(1, { message: "Page number must be greater than 0." })
  @Max(100, { message: "Page number exceeds maximum safe integer." })
  pageNumber: number;

  /** The number of items per page. */ @IsNumber()
  @Min(1, { message: "Page size must be greater than 0." })
  @Max(Number.MAX_VALUE, { message: "Page size exceeds maximum of 100." })
  pageSize: number;

  /**
   * Creates a new paged request.
   * @param pageNumber - The page number to retrieve (default: 1).
   * @param pageSize - The number of items per page (default: 10).
   */
  constructor(pageNumber: number = 1, pageSize: number = 10) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
  }
}
