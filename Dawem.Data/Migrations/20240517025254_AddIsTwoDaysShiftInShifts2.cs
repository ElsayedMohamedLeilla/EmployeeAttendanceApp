using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTwoDaysShiftInShifts2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTwoDaysShift",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftWorkingTimes_AllowedMinutes",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "AllowedMinutes");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftWorkingTimes_IsTwoDaysShift",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "IsTwoDaysShift");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDays_WeekDay",
                schema: "Dawem",
                table: "ScheduleDays",
                column: "WeekDay");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShiftWorkingTimes_AllowedMinutes",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_ShiftWorkingTimes_IsTwoDaysShift",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleDays_WeekDay",
                schema: "Dawem",
                table: "ScheduleDays");

            migrationBuilder.DropColumn(
                name: "IsTwoDaysShift",
                schema: "Dawem",
                table: "EmployeeAttendances");
        }
    }
}
