using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class DisableReasonInAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "VacationsTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "UserScreenActionPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "UserBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Translations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "TaskTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanDepartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "ScheduleDays",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "PermissionsTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "MyUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "JustificationsTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "JobTitles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "HolidayTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "GroupEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Currencies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "AssignmentTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisableReason",
                schema: "Dawem",
                table: "ActionLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "VacationsTypes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "UserScreenActionPermissions");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlans");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanGroups");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanEmployees");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanDepartments");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "ScheduleDays");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "PermissionsTypes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "JustificationsTypes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "GroupEmployees");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.DropColumn(
                name: "DisableReason",
                schema: "Dawem",
                table: "ActionLogs");
        }
    }
}
