import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, JoinColumn } from "typeorm";
import { BaseEntity } from "./BaseEntity";
import { Patient } from "./Patient";

@Entity("order")
export class Order extends BaseEntity {
  @PrimaryGeneratedColumn({ type: "bigint", name: "id" })
  id!: number;

  @Column({ type: "varchar", name: "message", length: 500 })
  message!: string;

  @Column({ type: "bigint", name: "patient_id" })
  patientId!: number;
  
  @ManyToOne(() => Patient, (patient) => patient.orders)
  @JoinColumn({ name: "patient_id" })
  patient!: Patient;
}
