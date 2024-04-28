using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFingerPrintDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FingerPrintDate",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TimeZoneToUTC",
                schema: "Dawem",
                table: "Countries",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FingerPrintDate",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropColumn(
                name: "TimeZoneToUTC",
                schema: "Dawem",
                table: "Countries");
        }
    }
}
