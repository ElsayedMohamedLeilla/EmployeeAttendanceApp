using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesInDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropColumn(
                name: "TimeZoneId",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "Dawem",
                table: "PermissionLogs",
                newName: "DateUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUTC",
                schema: "Dawem",
                table: "PermissionLogs",
                newName: "Date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneId",
                schema: "Dawem",
                table: "Countries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
