using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNameAndIsDeletedIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Zones",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "VacationTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "UserTokens",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "TaskTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Schedules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Sanctions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Roles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Responsibilities",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "PlanNameTranslations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "PermissionTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "MyUsers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "MenuItemNameTranslations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Languages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "JustificationTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "JobTitles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "HolidayTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Holidays",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Groups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "FingerprintDevices",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Employees",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Departments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "DefaultLookupsNameTranslations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "DefaultLookups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "CompanyIndustries",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "CompanyBranches",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Companies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "AssignmentTypes",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "UserTokens");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "PlanNameTranslations");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "MenuItemNameTranslations");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "DefaultLookupsNameTranslations");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "DefaultLookups");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "CompanyIndustries");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "CompanyBranches");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "AssignmentTypes");
        }
    }
}
