using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShiftId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Dawem.WeekAttendanceShifts");
            migrationBuilder.Sql("delete from Dawem.WeekAttendances");

            migrationBuilder.CreateIndex(
                name: "IX_WeekAttendanceShifts_ShiftId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeekAttendanceShifts_ShiftWorkingTimes_ShiftId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                column: "ShiftId",
                principalSchema: "Dawem",
                principalTable: "ShiftWorkingTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekAttendanceShifts_ShiftWorkingTimes_ShiftId",
                schema: "Dawem",
                table: "WeekAttendanceShifts");

            migrationBuilder.DropIndex(
                name: "IX_WeekAttendanceShifts_ShiftId",
                schema: "Dawem",
                table: "WeekAttendanceShifts");
        }
    }
}
