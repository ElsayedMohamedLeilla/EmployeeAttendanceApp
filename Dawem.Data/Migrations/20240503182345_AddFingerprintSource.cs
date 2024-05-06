using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFingerprintSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.AddColumn<int>(
                name: "FingerprintSource",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FingerprintSource",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.AddColumn<bool>(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
