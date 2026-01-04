export class PagedRequest {
    pageNumber: number;
    pageSize: number;

    constructor(pageNumber: number = 1, pageSize: number = 10) {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
    }
}
