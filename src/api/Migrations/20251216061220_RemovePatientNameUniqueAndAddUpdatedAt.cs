using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatientNameUniqueAndAddUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_patient_name",
                table: "patient");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "patient",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "order",
                type: "timestamptz",
                nullable: false,
                defaultValueSql: "NOW() AT TIME ZONE 'UTC'");

            migrationBuilder.CreateIndex(
                name: "IX_patient_name",
                table: "patient",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_patient_name",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "order");

            migrationBuilder.CreateIndex(
                name: "IX_patient_name",
                table: "patient",
                column: "name",
                unique: true);
        }
    }
}
