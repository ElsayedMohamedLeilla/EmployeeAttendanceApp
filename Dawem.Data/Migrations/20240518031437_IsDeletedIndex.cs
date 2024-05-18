using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Zones",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ZoneGroups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ZoneEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ZoneDepartments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "VacationTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "VacationBalances",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "UserResponsibilities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "UserBranches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Translations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "TaskTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonSanctions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Summons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonNotifyWays",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonLogSanctions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonGroups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonDepartments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Subscriptions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SubscriptionPayments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SubscriptionLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Settings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Schedules",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlans",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanGroups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanDepartments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ScheduleDays",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Sanctions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Responsibilities",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestVacations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestTasks",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestTaskEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Requests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestPermissions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestJustifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestAttachments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestAssignments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Plans",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PlanNameTranslations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionScreens",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionScreenActions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Permissions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationUserFCMTokens",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationTranslations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Notifications",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "MyUsers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Languages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "JustificationTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "JobTitles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "HolidayTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Holidays",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Groups",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "GroupManagerDelegators",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "GroupEmployees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDevices",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Employees",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "EmployeeOTPs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendances_FingerPrintStatus",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "FingerPrintStatus");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendances",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Departments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "DepartmentManagerDelegators",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Currencies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Countries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "CompanyIndustries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "CompanyBranches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "CompanyAttachments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Companies",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "AssignmentTypes",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ZoneGroups");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ZoneEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ZoneDepartments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "VacationBalances");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "UserResponsibilities");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonSanctions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonNotifyWays");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonLogSanctions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonLogs");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonGroups");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SummonDepartments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SubscriptionPayments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SubscriptionLogs");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlans");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanLogs");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanGroups");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlanDepartments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ScheduleDays");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestVacations");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestTasks");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestTaskEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestPermissions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestJustifications");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestAttachments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestAssignments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PlanNameTranslations");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionScreenActions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationUsers");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationUserFCMTokens");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationTranslations");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "NotificationEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "GroupManagerDelegators");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "GroupEmployees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "EmployeeOTPs");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendances_FingerPrintStatus",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "DepartmentManagerDelegators");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "CompanyIndustries");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "CompanyBranches");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "CompanyAttachments");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "AssignmentTypes");
        }
    }
}
