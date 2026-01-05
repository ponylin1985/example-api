/**
 * Result DTO for paginated queries.
 * @template T - The type of items in the result.
 */
export class PagedResult<T> {
  /** The items for the current page. */
  data: T[];
  /** The current page number. */
  pageNumber: number;
  /** The total number of pages available. */
  totalPages: number;
  /** The total count of all items across all pages. */
  totalCount: number;
  /** The number of items per page. */
  pageSize: number;

  /**
   * Creates a new paged result.
   * @param data - The items for the current page.
   * @param totalCount - The total count of all items across all pages.
   * @param pageNumber - The current page number.
   * @param pageSize - The number of items per page.
   */
  constructor(data: T[], totalCount: number, pageNumber: number, pageSize: number) {
    this.data = data;
    this.totalCount = totalCount;
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.totalPages = Math.ceil(totalCount / pageSize);
  }

  /**
   * Indicates whether there is a previous page available.
   */
  get hasPreviousPage(): boolean {
    return this.pageNumber > 1;
  }

  /**
   * Indicates whether there is a next page available.
   */
  get hasNextPage(): boolean {
    return this.pageNumber < this.totalPages;
  }
}
