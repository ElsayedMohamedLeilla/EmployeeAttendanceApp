using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowChangeFingerprintDeviceCodeForOneTimeToEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowChangeFingerprintDeviceCodeForOneTime",
                schema: "Dawem",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FingerprintDeviceCode",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendanceChecks_ZoneId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendanceChecks_Zones_ZoneId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "ZoneId",
                principalSchema: "Dawem",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendanceChecks_Zones_ZoneId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendanceChecks_ZoneId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropColumn(
                name: "AllowChangeFingerprintDeviceCodeForOneTime",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "FingerprintDeviceCode",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");
        }
    }
}
