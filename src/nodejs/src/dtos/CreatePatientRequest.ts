export class CreatePatientRequest {
    name: string;
    orderMessage: string;

    constructor(name: string, orderMessage: string) {
        this.name = name;
        this.orderMessage = orderMessage;
    }
}
