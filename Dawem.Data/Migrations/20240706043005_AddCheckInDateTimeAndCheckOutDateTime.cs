using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckInDateTimeAndCheckOutDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInDateTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "datetime2",
                nullable: true,
                computedColumnSql: "dbo.CheckInDateTime(Id)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutDateTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "datetime2",
                nullable: true,
                computedColumnSql: "dbo.CheckOutDateTime(Id)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInDateTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "CheckOutDateTime",
                schema: "Dawem",
                table: "EmployeeAttendances");
        }
    }
}
