using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class IndexOptimize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_patientorder_createdby_createdat",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patientorder_patientid",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patientorder_status",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patientorder_status_type",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patientorder_type",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patient_createdby_createdat",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "IX_patient_email",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "ix_patient_name_email_phoneNumber",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "IX_patient_phone_number",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "ix_medication_name",
                table: "medication");

            migrationBuilder.RenameIndex(
                name: "IX_prescription_order_id",
                table: "prescription",
                newName: "ix_prescription_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_prescription_medication_id",
                table: "prescription",
                newName: "ix_prescription_medication_id");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "prescription",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "frequency",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "dose",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "prescription",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "patient_order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "patient_order",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "instructions",
                table: "patient_order",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "patient_order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "patient_order",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "street",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "road",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "area",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "address_others",
                table: "patient",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "medication",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "manufacturer",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "medication",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "TIMEZONE('utc', NOW())",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_patientid",
                table: "patient_order",
                column: "patient_id")
                .Annotation("Npgsql:IndexInclude", new[] { "status", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_type_status_createdby_createdat",
                table: "patient_order",
                columns: new[] { "type", "status", "created_by", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_patient_createdby_createdat_name",
                table: "patient",
                columns: new[] { "created_at", "created_by", "name" })
                .Annotation("Npgsql:IndexInclude", new[] { "status" });

            migrationBuilder.CreateIndex(
                name: "ix_patient_email",
                table: "patient",
                column: "email",
                unique: true,
                filter: "email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_patient_phonenumber",
                table: "patient",
                column: "phone_number",
                unique: true,
                filter: "phone_number IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_medication_name",
                table: "medication",
                column: "name",
                unique: true,
                filter: "name IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_patientorder_patientid",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patientorder_type_status_createdby_createdat",
                table: "patient_order");

            migrationBuilder.DropIndex(
                name: "ix_patient_createdby_createdat_name",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "ix_patient_email",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "ix_patient_phonenumber",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "ix_medication_name",
                table: "medication");

            migrationBuilder.RenameIndex(
                name: "ix_prescription_order_id",
                table: "prescription",
                newName: "IX_prescription_order_id");

            migrationBuilder.RenameIndex(
                name: "ix_prescription_medication_id",
                table: "prescription",
                newName: "IX_prescription_medication_id");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "prescription",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "frequency",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "dose",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "prescription",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "prescription",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "patient_order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "patient_order",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "instructions",
                table: "patient_order",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "patient_order",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "patient_order",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "street",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "road",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "area",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "address_others",
                table: "patient",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "medication",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "manufacturer",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "medication",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created_at",
                table: "medication",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz",
                oldDefaultValueSql: "TIMEZONE('utc', NOW())");

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_createdby_createdat",
                table: "patient_order",
                columns: new[] { "created_by", "created_at" })
                .Annotation("Npgsql:IndexInclude", new[] { "patient_id", "instructions", "status", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_patientid",
                table: "patient_order",
                column: "patient_id")
                .Annotation("Npgsql:IndexInclude", new[] { "instructions", "status", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_status",
                table: "patient_order",
                column: "status")
                .Annotation("Npgsql:IndexInclude", new[] { "patient_id", "instructions", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_status_type",
                table: "patient_order",
                columns: new[] { "status", "type" })
                .Annotation("Npgsql:IndexInclude", new[] { "patient_id", "instructions" });

            migrationBuilder.CreateIndex(
                name: "ix_patientorder_type",
                table: "patient_order",
                column: "type")
                .Annotation("Npgsql:IndexInclude", new[] { "patient_id", "instructions", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_patient_createdby_createdat",
                table: "patient",
                columns: new[] { "created_by", "created_at" })
                .Annotation("Npgsql:IndexInclude", new[] { "age", "gender", "status", "name", "email", "phone_number" });

            migrationBuilder.CreateIndex(
                name: "IX_patient_email",
                table: "patient",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_patient_name_email_phoneNumber",
                table: "patient",
                columns: new[] { "name", "email", "phone_number" })
                .Annotation("Npgsql:IndexInclude", new[] { "age", "gender", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_patient_phone_number",
                table: "patient",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_medication_name",
                table: "medication",
                column: "name",
                unique: true);
        }
    }
}
