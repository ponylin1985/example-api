import { CreateDateColumn, UpdateDateColumn, ValueTransformer } from "typeorm";

/**
 * Transformer to ensure dates are always stored and retrieved as UTC.
 */
const utcDateTransformer: ValueTransformer = {
  to: (value: Date | undefined): Date | undefined => {
    return value;
  },
  from: (value: string | Date | undefined): Date | undefined => {
    return value ? new Date(value) : undefined;
  },
};

/**
 * Base entity class providing common timestamp fields for all entities.
 */
export abstract class BaseEntity {
  /**
   * The timestamp when the entity was created (stored as UTC in database).
   */
  @CreateDateColumn({
    type: "timestamptz",
    name: "created_at",
    transformer: utcDateTransformer,
  })
  createdAt!: Date;

  /**
   * The timestamp when the entity was last updated (stored as UTC in database).
   */
  @UpdateDateColumn({
    type: "timestamptz",
    name: "updated_at",
    transformer: utcDateTransformer,
  })
  updatedAt!: Date;
}
