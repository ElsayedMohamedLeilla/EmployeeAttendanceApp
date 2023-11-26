using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserVerifyEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendance_Companies_CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendance_Employees_EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendance_Schedules_ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendance_ShiftWorkingTimes_ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendance_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestVacations_Departments_VacationTypeId",
                schema: "Dawem",
                table: "RequestVacations");

            migrationBuilder.DropTable(
                name: "DepartmentZones",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_Groups_EmployeeId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ScheduleId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ShiftId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_EmployeeAttendance_TempId",
                schema: "Dawem",
                table: "EmployeeAttendance");

            migrationBuilder.DropColumn(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LocalDate",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ShiftCheckInTime",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ShiftCheckOutTime",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "EmployeeAttendance",
                schema: "Dawem",
                newName: "EmployeeAttendances",
                newSchema: "Dawem");

            migrationBuilder.RenameColumn(
                name: "TempId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                schema: "Dawem",
                table: "MyUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AddUserId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<int>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionDate",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LocalDate",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifyUserId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ShiftCheckInTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ShiftCheckOutTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAttendances",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VacationTypes",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_VacationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZoneDepartments",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ZoneDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZoneDepartments_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZoneEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ZoneEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZoneEmployees_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZoneGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ZoneGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZoneGroups_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_CompanyId",
                schema: "Dawem",
                table: "VacationTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneDepartments_DepartmentId",
                schema: "Dawem",
                table: "ZoneDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneDepartments_ZoneId",
                schema: "Dawem",
                table: "ZoneDepartments",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneEmployees_EmployeeId",
                schema: "Dawem",
                table: "ZoneEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneEmployees_ZoneId",
                schema: "Dawem",
                table: "ZoneEmployees",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneGroups_GroupId",
                schema: "Dawem",
                table: "ZoneGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneGroups_ZoneId",
                schema: "Dawem",
                table: "ZoneGroups",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "EmployeeAttendanceId",
                principalSchema: "Dawem",
                principalTable: "EmployeeAttendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendances_Companies_CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendances_Employees_EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendances_Schedules_ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "ScheduleId",
                principalSchema: "Dawem",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendances_ShiftWorkingTimes_ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "ShiftId",
                principalSchema: "Dawem",
                principalTable: "ShiftWorkingTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestVacations_VacationTypes_VacationTypeId",
                schema: "Dawem",
                table: "RequestVacations",
                column: "VacationTypeId",
                principalSchema: "Dawem",
                principalTable: "VacationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendances_Companies_CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendances_Employees_EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendances_Schedules_ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendances_ShiftWorkingTimes_ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestVacations_VacationTypes_VacationTypeId",
                schema: "Dawem",
                table: "RequestVacations");

            migrationBuilder.DropTable(
                name: "VacationTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneGroups",
                schema: "Dawem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAttendances",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "AddUserId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "AddedDate",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "DeletionDate",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "LocalDate",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "ModifyUserId",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "ShiftCheckInTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "ShiftCheckOutTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.RenameTable(
                name: "EmployeeAttendances",
                schema: "Dawem",
                newName: "EmployeeAttendance",
                newSchema: "Dawem");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "Dawem",
                table: "EmployeeAttendance",
                newName: "TempId");

            migrationBuilder.AddColumn<int>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LocalDate",
                schema: "Dawem",
                table: "Groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ShiftCheckInTime",
                schema: "Dawem",
                table: "Groups",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ShiftCheckOutTime",
                schema: "Dawem",
                table: "Groups",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_EmployeeAttendance_TempId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                column: "TempId");

            migrationBuilder.CreateTable(
                name: "DepartmentZones",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_DepartmentZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentZones_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentZones_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_EmployeeId",
                schema: "Dawem",
                table: "Groups",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ScheduleId",
                schema: "Dawem",
                table: "Groups",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ShiftId",
                schema: "Dawem",
                table: "Groups",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentZones_DepartmentId",
                schema: "Dawem",
                table: "DepartmentZones",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentZones_ZoneId",
                schema: "Dawem",
                table: "DepartmentZones",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendance_Companies_CompanyId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendance_Employees_EmployeeId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendance_Schedules_ScheduleId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                column: "ScheduleId",
                principalSchema: "Dawem",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendance_ShiftWorkingTimes_ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendance",
                column: "ShiftId",
                principalSchema: "Dawem",
                principalTable: "ShiftWorkingTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendanceChecks_EmployeeAttendance_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "EmployeeAttendanceId",
                principalSchema: "Dawem",
                principalTable: "EmployeeAttendance",
                principalColumn: "TempId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_RequestVacations_Departments_VacationTypeId",
                schema: "Dawem",
                table: "RequestVacations",
                column: "VacationTypeId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
