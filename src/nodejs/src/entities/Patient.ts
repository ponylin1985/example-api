import { Entity, PrimaryGeneratedColumn, Column, OneToMany } from "typeorm";
import { BaseEntity } from "./BaseEntity";
import { Order } from "./Order";

@Entity()
export class Patient extends BaseEntity {
  @PrimaryGeneratedColumn({ type: "bigint" })
  id!: string;

  @Column()
  name!: string;

  @OneToMany(() => Order, (order) => order.patient)
  orders!: Order[];
}
