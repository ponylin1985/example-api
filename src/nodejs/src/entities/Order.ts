import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, JoinColumn } from "typeorm";
import { BaseEntity } from "./BaseEntity";
import { Patient } from "./Patient";

@Entity()
export class Order extends BaseEntity {
  @PrimaryGeneratedColumn({ type: "bigint" })
  id!: string;

  @Column()
  message!: string;

  @Column({ type: "bigint" })
  patientId!: string;

  @ManyToOne(() => Patient, (patient) => patient.orders)
  @JoinColumn({ name: "patientId" })
  patient!: Patient;
}
