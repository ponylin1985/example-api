import { ApiCode } from "./ApiCode";

export class ApiResult {
    success: boolean;
    code: ApiCode;
    message: string;

    constructor(success: boolean, code: ApiCode, message: string = "") {
        this.success = success;
        this.code = code;
        this.message = message;
    }
}

export class ApiDataResult<T> extends ApiResult {
    data?: T;

    constructor(success: boolean, code: ApiCode, message: string = "", data?: T) {
        super(success, code, message);
        this.data = data;
    }
}
