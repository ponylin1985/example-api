using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AlterPatientAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_patient",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "IX_patient_name",
                table: "patient");

            migrationBuilder.AddColumn<string>(
                name: "address_others",
                table: "patient",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "patient",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "area",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "date_of_birth",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "patient",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "first_visit_date",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "gender",
                table: "patient",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "patient",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "patient",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "road",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "status",
                table: "patient",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "street",
                table: "patient",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "patient",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "pk_patient_id",
                table: "patient",
                column: "id");

            migrationBuilder.CreateTable(
                name: "medication",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    manufacturer = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medication", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "patient_order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    instructions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, defaultValue: ""),
                    next_visit_date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    start_date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    end_date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    type = table.Column<byte>(type: "smallint", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    dispensed_date = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    patient_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_patient_order_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prescription",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    medication_id = table.Column<long>(type: "bigint", nullable: false),
                    dose = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    frequency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    duration_in_days = table.Column<int>(type: "integer", nullable: false),
                    route = table.Column<byte>(type: "smallint", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prescription", x => x.id);
                    table.ForeignKey(
                        name: "FK_prescription_medication_medication_id",
                        column: x => x.medication_id,
                        principalTable: "medication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prescription_patient_order_order_id",
                        column: x => x.order_id,
                        principalTable: "patient_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                column: "name");

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
                name: "IX_prescription_medication_id",
                table: "prescription",
                column: "medication_id");

            migrationBuilder.CreateIndex(
                name: "IX_prescription_order_id",
                table: "prescription",
                column: "order_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prescription");

            migrationBuilder.DropTable(
                name: "medication");

            migrationBuilder.DropTable(
                name: "patient_order");

            migrationBuilder.DropPrimaryKey(
                name: "pk_patient_id",
                table: "patient");

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

            migrationBuilder.DropColumn(
                name: "address_others",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "age",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "area",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "city",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "country",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "email",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "first_visit_date",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "remarks",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "road",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "status",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "street",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "patient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_patient",
                table: "patient",
                column: "id");

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patient_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_patient_patient_id",
                        column: x => x.patient_id,
                        principalTable: "patient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patient_name",
                table: "patient",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PatientId",
                table: "order",
                column: "patient_id");
        }
    }
}
