using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestAssignments_Employees_EmployeeId",
                schema: "Dawem",
                table: "RequestAssignments");

            migrationBuilder.DropIndex(
                name: "IX_RequestAssignments_EmployeeId",
                schema: "Dawem",
                table: "RequestAssignments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Dawem",
                table: "RequestAssignments");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                schema: "Dawem",
                table: "Zones",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                schema: "Dawem",
                table: "Zones",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);

            migrationBuilder.AddColumn<int>(
                name: "RecognitionWay",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecognitionWay",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                schema: "Dawem",
                table: "Zones",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                schema: "Dawem",
                table: "Zones",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "Dawem",
                table: "RequestAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestAssignments_EmployeeId",
                schema: "Dawem",
                table: "RequestAssignments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestAssignments_Employees_EmployeeId",
                schema: "Dawem",
                table: "RequestAssignments",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
