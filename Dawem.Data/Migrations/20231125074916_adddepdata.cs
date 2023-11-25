using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class adddepdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentManagerDelegators_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentManagerDelegators");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentZones_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentZones");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "DepartmentEmployees",
                schema: "Dawem");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentManagerDelegators_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentManagerDelegators",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Dawem",
                table: "Departments",
                column: "ManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentZones_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentZones",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentManagerDelegators_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentManagerDelegators");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentZones_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentZones");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.CreateTable(
                name: "DepartmentEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentEmployees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEmployees_DepartmentId",
                schema: "Dawem",
                table: "DepartmentEmployees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEmployees_EmployeeId",
                schema: "Dawem",
                table: "DepartmentEmployees",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentManagerDelegators_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentManagerDelegators",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Dawem",
                table: "Departments",
                column: "ManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentZones_Departments_DepartmentId",
                schema: "Dawem",
                table: "DepartmentZones",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
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
    }
}
