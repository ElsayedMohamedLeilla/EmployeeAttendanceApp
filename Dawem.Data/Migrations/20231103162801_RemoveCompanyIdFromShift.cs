using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanyIdFromShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeekAttendanceShifts_Companies_CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts");

            migrationBuilder.DropIndex(
                name: "IX_WeekAttendanceShifts_CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WeekAttendanceShifts_CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeekAttendanceShifts_Companies_CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
