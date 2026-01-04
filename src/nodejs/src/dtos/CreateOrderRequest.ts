export class CreateOrderRequest {
    patientId: number;
    message: string;

    constructor(patientId: number, message: string) {
        this.patientId = patientId;
        this.message = message;
    }
}
