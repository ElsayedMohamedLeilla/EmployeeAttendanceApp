using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPopsToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnualVacationBalance",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttendanceType",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JobTitleId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualVacationBalance",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AttendanceType",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                schema: "Dawem",
                table: "Employees");
        }
    }
}
