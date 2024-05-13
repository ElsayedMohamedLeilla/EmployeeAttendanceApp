using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanLogs");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "SchedulePlanLogs");

            //migrationBuilder.CreateTable(
            //    name: "EmployeeDailyAttendanceGroupByDayReport",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        EmployeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        JobTitleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        TotalWorkingHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
            //        TotalLateArrivalsHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
            //        TotalEarlyDeparturesHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
            //        TotalOverTimeHours = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanLogs_CompanyId",
                schema: "Dawem",
                table: "SchedulePlanLogs",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDailyAttendanceGroupByDayReport",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_SchedulePlanLogs_CompanyId",
                schema: "Dawem",
                table: "SchedulePlanLogs");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "SchedulePlanLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanLogs",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }
    }
}
