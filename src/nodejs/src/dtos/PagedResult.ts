export class PagedResult<T> {
    data: T[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    pageSize: number;

    constructor(data: T[], totalCount: number, pageNumber: number, pageSize: number) {
        this.data = data;
        this.totalCount = totalCount;
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.totalPages = Math.ceil(totalCount / pageSize);
    }

    get hasPreviousPage(): boolean {
        return this.pageNumber > 1;
    }

    get hasNextPage(): boolean {
        return this.pageNumber < this.totalPages;
    }
}
