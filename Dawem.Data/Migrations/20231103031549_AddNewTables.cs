using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "VacationsTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "UserScreenActionPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "UserGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "UserBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Translations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "TaskTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "PermissionsTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "JustificationsTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "HolidayTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Currencies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "AssignmentTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "Dawem",
                table: "ActionLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WeekAttendances",
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
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeekAttendances_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeekAttendanceShifts",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    WeekAttendanceId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: true),
                    WeekDay = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekAttendanceShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeekAttendanceShifts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeekAttendanceShifts_WeekAttendances_WeekAttendanceId",
                        column: x => x.WeekAttendanceId,
                        principalSchema: "Dawem",
                        principalTable: "WeekAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeekAttendances_CompanyId",
                schema: "Dawem",
                table: "WeekAttendances",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekAttendanceShifts_CompanyId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekAttendanceShifts_WeekAttendanceId",
                schema: "Dawem",
                table: "WeekAttendanceShifts",
                column: "WeekAttendanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekAttendanceShifts",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "WeekAttendances",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "VacationsTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "UserScreenActionPermissions");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "PermissionsTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "JustificationsTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "Dawem",
                table: "ActionLogs");
        }
    }
}
