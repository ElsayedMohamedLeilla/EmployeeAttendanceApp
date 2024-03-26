using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleComputedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalEarlyDeparturesHours",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: true,
                computedColumnSql: "dbo.TotalEarlyDeparturesHours(Id)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalLateArrivalsHours",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: true,
                computedColumnSql: "dbo.TotalLateArrivalsHours(Id)");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalOverTimeHours",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: true,
                computedColumnSql: "dbo.TotalOverTimeHours(Id)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalEarlyDeparturesHours",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "TotalLateArrivalsHours",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "TotalOverTimeHours",
                schema: "Dawem",
                table: "EmployeeAttendances");
        }
    }
}
