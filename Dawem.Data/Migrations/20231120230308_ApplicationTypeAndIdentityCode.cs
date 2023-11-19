using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTypeAndIdentityCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "VacationsTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "VacationsTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "UserScreenActionPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "UserScreenActionPermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "UserBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "UserBranches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Translations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Translations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "TaskTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "TaskTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanDepartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanDepartments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "ScheduleDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "ScheduleDays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "PermissionsTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "PermissionsTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "JustificationsTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "JustificationsTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "JobTitles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "JobTitles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "HolidayTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "HolidayTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "GroupEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "GroupEmployees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Currencies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Countries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneId",
                schema: "Dawem",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdentityCode",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Branches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Branches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "AssignmentTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "AssignmentTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "ActionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "ActionLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IdentityCode",
                schema: "Dawem",
                table: "Companies",
                column: "IdentityCode",
                unique: true,
                filter: "[IdentityCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_IdentityCode",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "VacationsTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "VacationsTypes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "UserScreenActionPermissions");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "UserScreenActionPermissions");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlans");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlans");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanGroups");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanGroups");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanEmployees");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanEmployees");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanDepartments");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanDepartments");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "ScheduleDays");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "ScheduleDays");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "PermissionsTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "PermissionsTypes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "JustificationsTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "JustificationsTypes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "GroupEmployees");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "GroupEmployees");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "TimeZoneId",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IdentityCode",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "ActionLogs");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "ActionLogs");
        }
    }
}
