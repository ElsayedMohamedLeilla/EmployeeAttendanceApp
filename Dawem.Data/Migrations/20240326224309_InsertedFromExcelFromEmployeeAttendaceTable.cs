using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsertedFromExcelFromEmployeeAttendaceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");
        }
    }
}
