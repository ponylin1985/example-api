export class OrderDto {
    id: number;
    message: string;
    patientId: number;
    createdAt: Date;
    updatedAt?: Date;

    constructor(id: number, message: string, patientId: number, createdAt: Date, updatedAt?: Date) {
        this.id = id;
        this.message = message;
        this.patientId = patientId;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }
}
