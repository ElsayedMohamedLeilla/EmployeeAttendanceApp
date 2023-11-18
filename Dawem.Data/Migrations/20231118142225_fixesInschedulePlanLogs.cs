using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixesInschedulePlanLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchedulePlanBackgroundJobLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SchedulePlanId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    SchedulePlanType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_SchedulePlanBackgroundJobLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlanBackgroundJobLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlanBackgroundJobLogs_SchedulePlans_SchedulePlanId",
                        column: x => x.SchedulePlanId,
                        principalSchema: "Dawem",
                        principalTable: "SchedulePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePlanBackgroundJobLogEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulePlanBackgroundJobLogId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OldScheduleId = table.Column<int>(type: "int", nullable: true),
                    NewScheduleId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SchedulePlanBackgroundJobLogEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlanBackgroundJobLogEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlanBackgroundJobLogEmployees_SchedulePlanBackgroundJobLogs_SchedulePlanBackgroundJobLogId",
                        column: x => x.SchedulePlanBackgroundJobLogId,
                        principalSchema: "Dawem",
                        principalTable: "SchedulePlanBackgroundJobLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchedulePlanBackgroundJobLogEmployees_Schedules_NewScheduleId",
                        column: x => x.NewScheduleId,
                        principalSchema: "Dawem",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlanBackgroundJobLogEmployees_Schedules_OldScheduleId",
                        column: x => x.OldScheduleId,
                        principalSchema: "Dawem",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanBackgroundJobLogEmployees_EmployeeId",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanBackgroundJobLogEmployees_NewScheduleId",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                column: "NewScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanBackgroundJobLogEmployees_OldScheduleId",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                column: "OldScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanBackgroundJobLogEmployees_SchedulePlanBackgroundJobLogId",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogEmployees",
                column: "SchedulePlanBackgroundJobLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanBackgroundJobLogs_CompanyId",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanBackgroundJobLogs_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanBackgroundJobLogs",
                column: "SchedulePlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedulePlanBackgroundJobLogEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanBackgroundJobLogs",
                schema: "Dawem");
        }
    }
}
