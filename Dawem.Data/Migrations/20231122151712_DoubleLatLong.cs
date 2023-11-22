using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class DoubleLatLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
