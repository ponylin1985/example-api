import { Entity, PrimaryGeneratedColumn, Column, ManyToOne, JoinColumn } from "typeorm";
import { BaseEntity } from "./BaseEntity";
import { Patient } from "./Patient";

/**
 * Entity representing an order in the system.
 */
@Entity("order")
export class Order extends BaseEntity {
  /**
   * The unique identifier for the order.
   */
  @PrimaryGeneratedColumn({ type: "bigint", name: "id" })
  id!: number;

  /**
   * The message associated with the order.
   */
  @Column({ type: "varchar", name: "message", length: 500 })
  message!: string;

  /**
   * The ID of the patient who owns this order.
   */
  @Column({ type: "bigint", name: "patient_id" })
  patientId!: number;

  /**
   * The patient who owns this order.
   */
  @ManyToOne(() => Patient, (patient) => patient.orders)
  @JoinColumn({ name: "patient_id" })
  patient!: Patient;
}
