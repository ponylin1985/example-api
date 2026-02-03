using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientOrderHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "patient_order_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    patient_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    remarks = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    log_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    log_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "TIMEZONE('utc', NOW())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_order_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_patient_order_history_order_id",
                        column: x => x.order_id,
                        principalTable: "patient_order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_patient_order_history_order_patient",
                table: "patient_order_history",
                columns: new[] { "order_id", "patient_id" })
                .Annotation("Npgsql:IndexInclude", new[] { "status", "log_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patient_order_history");
        }
    }
}
