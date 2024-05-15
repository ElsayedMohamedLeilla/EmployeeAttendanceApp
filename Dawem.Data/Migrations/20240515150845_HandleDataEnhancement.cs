using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleDataEnhancement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HelperId",
                schema: "Dawem",
                table: "Notifications",
                newName: "HelperNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_HelperId",
                schema: "Dawem",
                table: "Notifications",
                newName: "IX_Notifications_HelperNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "HelperDate",
                schema: "Dawem",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftWorkingTimes_CheckInTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "CheckInTime");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftWorkingTimes_CheckOutTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "CheckOutTime");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HelperDate",
                schema: "Dawem",
                table: "Notifications",
                column: "HelperDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_LocalDate",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "LocalDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_ShiftCheckInTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "ShiftCheckInTime");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_ShiftCheckOutTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "ShiftCheckOutTime");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendanceChecks_FingerPrintDate",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "FingerPrintDate");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendanceChecks_FingerPrintDateUTC",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "FingerPrintDateUTC");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendanceChecks_FingerPrintType",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "FingerPrintType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShiftWorkingTimes_CheckInTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_ShiftWorkingTimes_CheckOutTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_HelperDate",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_LocalDate",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_ShiftCheckInTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_ShiftCheckOutTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendanceChecks_FingerPrintDate",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendanceChecks_FingerPrintDateUTC",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendanceChecks_FingerPrintType",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropColumn(
                name: "HelperDate",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "HelperNumber",
                schema: "Dawem",
                table: "Notifications",
                newName: "HelperId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_HelperNumber",
                schema: "Dawem",
                table: "Notifications",
                newName: "IX_Notifications_HelperId");
        }
    }
}
