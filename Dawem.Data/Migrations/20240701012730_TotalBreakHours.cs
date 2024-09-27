using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class TotalBreakHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalBreakHours",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: true,
                computedColumnSql: "dbo.TotalBreakHours(Id)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBreakHours",
                schema: "Dawem",
                table: "EmployeeAttendances");
        }
    }
}
