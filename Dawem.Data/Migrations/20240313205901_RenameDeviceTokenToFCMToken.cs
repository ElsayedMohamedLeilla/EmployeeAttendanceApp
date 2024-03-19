using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDeviceTokenToFCMToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationUserDeviceTokens",
                schema: "Dawem");

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

            migrationBuilder.CreateTable(
                name: "NotificationUserFCMTokens",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationUserId = table.Column<int>(type: "int", nullable: false),
                    FCMToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    LastLogInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_NotificationUserFCMTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUserFCMTokens_NotificationUsers_NotificationUserId",
                        column: x => x.NotificationUserId,
                        principalSchema: "Dawem",
                        principalTable: "NotificationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Zones",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "VacationTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "TaskTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Schedules",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Sanctions",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "PermissionTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "MyUsers",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[CompanyId] IS NOT NULL AND [Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "JustificationTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "JobTitles",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "HolidayTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Holidays",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Groups",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDevices",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Employees",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Departments",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Branches",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "AssignmentTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserFCMTokens_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens",
                column: "NotificationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationUserFCMTokens",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "VacationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "ShiftWorkingTimes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Sanctions");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "PermissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "JustificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "JobTitles");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "HolidayTypes");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "AssignmentTypes");

            migrationBuilder.CreateTable(
                name: "NotificationUserDeviceTokens",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationUserId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastLogInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUserDeviceTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId",
                        column: x => x.NotificationUserId,
                        principalSchema: "Dawem",
                        principalTable: "NotificationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokens_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId");
        }
    }
}
