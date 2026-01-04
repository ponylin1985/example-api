import { OrderDto } from "./OrderDto";

export class PatientDto {
    id: number;
    name: string;
    createdAt: Date;
    updatedAt?: Date;
    orders: OrderDto[];

    constructor(id: number, name: string, createdAt: Date, orders: OrderDto[] = [], updatedAt?: Date) {
        this.id = id;
        this.name = name;
        this.createdAt = createdAt;
        this.orders = orders;
        this.updatedAt = updatedAt;
    }
}
