import { Entity, PrimaryGeneratedColumn, Column, OneToMany } from "typeorm";
import { BaseEntity } from "./BaseEntity";
import { Order } from "./Order";

/**
 * Entity representing a patient in the system.
 */
@Entity("patient")
export class Patient extends BaseEntity {
  /**
   * The unique identifier for the patient.
   */
  @PrimaryGeneratedColumn({ type: "bigint", name: "id" })
  id!: number;

  /**
   * The name of the patient.
   */
  @Column({ type: "varchar", name: "name", length: 50 })
  name!: string;

  /**
   * The collection of orders associated with this patient.
   */
  @OneToMany(() => Order, (order) => order.patient, { cascade: true })
  orders!: Order[];
}
