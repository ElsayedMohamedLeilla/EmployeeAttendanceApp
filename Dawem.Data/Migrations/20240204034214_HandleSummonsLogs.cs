using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleSummonsLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sanctions_Companies_CompanyId",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropForeignKey(
                name: "FK_SummonDepartments_Departments_DepartmentId",
                schema: "Dawem",
                table: "SummonDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_SummonEmployees_Employees_EmployeeId",
                schema: "Dawem",
                table: "SummonEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_SummonGroups_Groups_GroupId",
                schema: "Dawem",
                table: "SummonGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Summons_Companies_CompanyId",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonSanctions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "Summons",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonNotifyWays",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonGroups",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonEmployees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonDepartments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "Sanctions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "SummonId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SummonMissingLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    DoneNotify = table.Column<bool>(type: "bit", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMissingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogs_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendanceChecks_SummonId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogs_CompanyId",
                schema: "Dawem",
                table: "SummonMissingLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogs_EmployeeId",
                schema: "Dawem",
                table: "SummonMissingLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogs_SummonId",
                schema: "Dawem",
                table: "SummonMissingLogs",
                column: "SummonId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendanceChecks_Summons_SummonId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "SummonId",
                principalSchema: "Dawem",
                principalTable: "Summons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sanctions_Companies_CompanyId",
                schema: "Dawem",
                table: "Sanctions",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SummonDepartments_Departments_DepartmentId",
                schema: "Dawem",
                table: "SummonDepartments",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SummonEmployees_Employees_EmployeeId",
                schema: "Dawem",
                table: "SummonEmployees",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SummonGroups_Groups_GroupId",
                schema: "Dawem",
                table: "SummonGroups",
                column: "GroupId",
                principalSchema: "Dawem",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Summons_Companies_CompanyId",
                schema: "Dawem",
                table: "Summons",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendanceChecks_Summons_SummonId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_Sanctions_Companies_CompanyId",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropForeignKey(
                name: "FK_SummonDepartments_Departments_DepartmentId",
                schema: "Dawem",
                table: "SummonDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_SummonEmployees_Employees_EmployeeId",
                schema: "Dawem",
                table: "SummonEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_SummonGroups_Groups_GroupId",
                schema: "Dawem",
                table: "SummonGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Summons_Companies_CompanyId",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropTable(
                name: "SummonMissingLogs",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendanceChecks_SummonId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropColumn(
                name: "SummonId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonSanctions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "Summons",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonNotifyWays",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonGroups",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonEmployees",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "SummonDepartments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "Sanctions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Dawem",
                table: "Sanctions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sanctions_Companies_CompanyId",
                schema: "Dawem",
                table: "Sanctions",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SummonDepartments_Departments_DepartmentId",
                schema: "Dawem",
                table: "SummonDepartments",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SummonEmployees_Employees_EmployeeId",
                schema: "Dawem",
                table: "SummonEmployees",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SummonGroups_Groups_GroupId",
                schema: "Dawem",
                table: "SummonGroups",
                column: "GroupId",
                principalSchema: "Dawem",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Summons_Companies_CompanyId",
                schema: "Dawem",
                table: "Summons",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
