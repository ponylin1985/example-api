import { Entity, PrimaryGeneratedColumn, Column, OneToMany } from "typeorm";
import { BaseEntity } from "./BaseEntity";
import { Order } from "./Order";

@Entity("patient")
export class Patient extends BaseEntity {
  @PrimaryGeneratedColumn({ type: "bigint", name: "id" })
  id!: number;

  @Column({ type: "varchar", name: "name", length: 50 })
  name!: string;

  @OneToMany(() => Order, (order) => order.patient, { cascade: true })
  orders!: Order[];
}
