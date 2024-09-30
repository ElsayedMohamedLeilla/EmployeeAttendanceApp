using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFingerprintDeviceLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "DefaultType",
            //    schema: "Dawem",
            //    table: "DefaultLookups");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "LastSeenDateUTC",
            //    schema: "Dawem",
            //    table: "FingerprintDevices",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<int>(
            //    name: "FingerprintDeviceUserCode",
            //    schema: "Dawem",
            //    table: "Employees",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "ShiftId",
            //    schema: "Dawem",
            //    table: "EmployeeAttendances",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.CreateTable(
                name: "FingerprintDeviceLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DeviceSerialNumber = table.Column<int>(type: "int", nullable: false),
                    LogType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestBody = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeviceLogType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerprintDeviceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintDeviceLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateTable(
            //    name: "FingerprintTransactions",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyId = table.Column<int>(type: "int", nullable: false),
            //        EmployeeId = table.Column<int>(type: "int", nullable: false),
            //        ScheduleId = table.Column<int>(type: "int", nullable: true),
            //        FingerprintDeviceId = table.Column<int>(type: "int", nullable: false),
            //        FingerprintDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        FingerprintUserId = table.Column<int>(type: "int", nullable: false),
            //        SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        FingerPrintType = table.Column<int>(type: "int", nullable: false),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        AddedApplicationType = table.Column<int>(type: "int", nullable: false),
            //        ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
            //        AddUserId = table.Column<int>(type: "int", nullable: true),
            //        ModifyUserId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FingerprintTransactions", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_FingerprintTransactions_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalSchema: "Dawem",
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FingerprintTransactions_Employees_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalSchema: "Dawem",
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FingerprintTransactions_FingerprintDevices_FingerprintDeviceId",
            //            column: x => x.FingerprintDeviceId,
            //            principalSchema: "Dawem",
            //            principalTable: "FingerprintDevices",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FingerprintTransactions_Schedules_ScheduleId",
            //            column: x => x.ScheduleId,
            //            principalSchema: "Dawem",
            //            principalTable: "Schedules",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintDeviceLogs_CompanyId",
                schema: "Dawem",
                table: "FingerprintDeviceLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "FingerprintDeviceLogs",
                column: "IsDeleted");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FingerprintTransactions_CompanyId",
            //    schema: "Dawem",
            //    table: "FingerprintTransactions",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FingerprintTransactions_EmployeeId",
            //    schema: "Dawem",
            //    table: "FingerprintTransactions",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FingerprintTransactions_FingerprintDeviceId",
            //    schema: "Dawem",
            //    table: "FingerprintTransactions",
            //    column: "FingerprintDeviceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FingerprintTransactions_ScheduleId",
            //    schema: "Dawem",
            //    table: "FingerprintTransactions",
            //    column: "ScheduleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_IsDeleted",
            //    schema: "Dawem",
            //    table: "FingerprintTransactions",
            //    column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FingerprintDeviceLogs",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintTransactions",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "LastSeenDateUTC",
                schema: "Dawem",
                table: "FingerprintDevices");

            migrationBuilder.DropColumn(
                name: "FingerprintDeviceUserCode",
                schema: "Dawem",
                table: "Employees");

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

            migrationBuilder.AddColumn<int>(
                name: "DefaultType",
                schema: "Dawem",
                table: "DefaultLookups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
