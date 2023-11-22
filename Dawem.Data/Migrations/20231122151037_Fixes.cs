using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.AlterColumn<int>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);

            migrationBuilder.AlterColumn<int>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "EmployeeAttendanceId",
                principalSchema: "Dawem",
                principalTable: "EmployeeAttendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.AlterColumn<decimal>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "EmployeeAttendanceId",
                principalSchema: "Dawem",
                principalTable: "EmployeeAttendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
