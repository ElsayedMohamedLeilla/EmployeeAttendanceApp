using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DirectManagerId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DirectManagerId",
                schema: "Dawem",
                table: "Employees",
                column: "DirectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_DirectManagerId",
                schema: "Dawem",
                table: "Employees",
                column: "DirectManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_DirectManagerId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DirectManagerId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DirectManagerId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                schema: "Dawem",
                table: "Employees");
        }
    }
}
