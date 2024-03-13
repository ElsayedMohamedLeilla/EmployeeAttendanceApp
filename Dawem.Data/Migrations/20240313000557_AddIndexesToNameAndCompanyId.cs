using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToNameAndCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Zones_CompanyId",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_VacationTypes_CompanyId",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_TaskTypes_CompanyId",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_ShiftWorkingTimes_CompanyId",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_CompanyId",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Sanctions_CompanyId",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_PermissionTypes_CompanyId",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_MyUsers_CompanyId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_JustificationTypes_CompanyId",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_JobTitles_CompanyId",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_HolidayTypes_CompanyId",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_Holidays_CompanyId",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CompanyId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_FingerprintDevices_CompanyId",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CompanyId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CompanyId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CompanyId",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentTypes_CompanyId",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Zones",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "VacationTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "TaskTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Schedules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Sanctions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "PermissionTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "MyUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "JustificationTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "JobTitles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "HolidayTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Holidays",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "FingerprintDevices",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Branches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "AssignmentTypes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Zones",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "VacationTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "TaskTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Schedules",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Sanctions",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "PermissionTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "MyUsers",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[CompanyId] IS NOT NULL AND [Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "JustificationTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "JobTitles",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "HolidayTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Holidays",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Groups",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "FingerprintDevices",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Employees",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Departments",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Branches",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "AssignmentTypes",
                columns: new[] { "CompanyId", "Name" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Zones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "VacationTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "TaskTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Sanctions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "PermissionTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "MyUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "JustificationTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "JobTitles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "HolidayTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Holidays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "FingerprintDevices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "AssignmentTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_CompanyId",
                schema: "Dawem",
                table: "Zones",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationTypes_CompanyId",
                schema: "Dawem",
                table: "VacationTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTypes_CompanyId",
                schema: "Dawem",
                table: "TaskTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftWorkingTimes_CompanyId",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_CompanyId",
                schema: "Dawem",
                table: "Schedules",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sanctions_CompanyId",
                schema: "Dawem",
                table: "Sanctions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionTypes_CompanyId",
                schema: "Dawem",
                table: "PermissionTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MyUsers_CompanyId",
                schema: "Dawem",
                table: "MyUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JustificationTypes_CompanyId",
                schema: "Dawem",
                table: "JustificationTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_CompanyId",
                schema: "Dawem",
                table: "JobTitles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayTypes_CompanyId",
                schema: "Dawem",
                table: "HolidayTypes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_CompanyId",
                schema: "Dawem",
                table: "Holidays",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CompanyId",
                schema: "Dawem",
                table: "Groups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintDevices_CompanyId",
                schema: "Dawem",
                table: "FingerprintDevices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                schema: "Dawem",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                schema: "Dawem",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyId",
                schema: "Dawem",
                table: "Branches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentTypes_CompanyId",
                schema: "Dawem",
                table: "AssignmentTypes",
                column: "CompanyId");
        }
    }
}
