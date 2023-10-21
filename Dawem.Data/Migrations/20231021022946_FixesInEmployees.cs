using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesInEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DapartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "DapartmentId",
                schema: "Dawem",
                table: "Employees",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DapartmentId",
                schema: "Dawem",
                table: "Employees",
                newName: "IX_Employees_DepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                schema: "Dawem",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                schema: "Dawem",
                table: "Employees",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                schema: "Dawem",
                table: "Employees",
                newName: "DapartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DepartmentId",
                schema: "Dawem",
                table: "Employees",
                newName: "IX_Employees_DapartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DapartmentId",
                schema: "Dawem",
                table: "Employees",
                column: "DapartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
