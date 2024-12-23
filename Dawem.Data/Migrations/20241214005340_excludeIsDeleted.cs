using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class excludeIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "VacationBalances");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlans");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "OvertimeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Zones",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "VacationTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "VacationBalances",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "TaskTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Summons",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Subscriptions",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Schedules",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "SchedulePlans",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Sanctions",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Responsibilities",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Requests",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "PermissionTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Permissions",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "OvertimeTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "MyUsers",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "JustificationTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "JobTitles",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "HolidayTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Holidays",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Groups",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "FingerprintDevices",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Employees",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "EmployeeAttendances",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Departments",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "AssignmentTypes",
                columns: new[] { "CompanyId", "Code" },
                unique: true,
                filter: "[IsDeleted]=0 and [CompanyId] is not null");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "VacationBalances");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "SchedulePlans");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "OvertimeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Zones",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "VacationTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "VacationBalances",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "TaskTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Summons",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Subscriptions",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Schedules",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "SchedulePlans",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Sanctions",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Responsibilities",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Requests",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "PermissionTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Permissions",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "OvertimeTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "MyUsers",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "JustificationTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "JobTitles",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "HolidayTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Holidays",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Groups",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDevices",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Employees",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "EmployeeAttendances",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Departments",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "AssignmentTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }
    }
}
