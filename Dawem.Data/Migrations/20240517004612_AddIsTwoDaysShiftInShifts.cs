using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTwoDaysShiftInShifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "EmployeeDailyAttendanceGroupByDayReport",
            //    schema: "Dawem");

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoDaysShift",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTwoDaysShift",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.CreateTable(
                name: "EmployeeDailyAttendanceGroupByDayReport",
                schema: "Dawem",
                columns: table => new
                {
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalEarlyDeparturesHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
                    TotalLateArrivalsHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
                    TotalOverTimeHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
                    TotalWorkingHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false)
                },
                constraints: table =>
                {
                });
        }
    }
}
