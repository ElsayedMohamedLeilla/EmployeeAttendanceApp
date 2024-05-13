using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleSchedulePlanLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulePlanLogs_SchedulePlans_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulePlanLogs_SchedulePlans_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanLogs",
                column: "SchedulePlanId",
                principalSchema: "Dawem",
                principalTable: "SchedulePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulePlanLogs_SchedulePlans_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulePlanLogs_SchedulePlans_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanLogs",
                column: "SchedulePlanId",
                principalSchema: "Dawem",
                principalTable: "SchedulePlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
