using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameLogColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulePlanLogEmployees_SchedulePlanLogs_SchedulePlanBackgroundJobLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees");

            migrationBuilder.RenameColumn(
                name: "SchedulePlanBackgroundJobLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                newName: "SchedulePlanLogId");

            migrationBuilder.RenameIndex(
                name: "IX_SchedulePlanLogEmployees_SchedulePlanBackgroundJobLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                newName: "IX_SchedulePlanLogEmployees_SchedulePlanLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulePlanLogEmployees_SchedulePlanLogs_SchedulePlanLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                column: "SchedulePlanLogId",
                principalSchema: "Dawem",
                principalTable: "SchedulePlanLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulePlanLogEmployees_SchedulePlanLogs_SchedulePlanLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees");

            migrationBuilder.RenameColumn(
                name: "SchedulePlanLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                newName: "SchedulePlanBackgroundJobLogId");

            migrationBuilder.RenameIndex(
                name: "IX_SchedulePlanLogEmployees_SchedulePlanLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                newName: "IX_SchedulePlanLogEmployees_SchedulePlanBackgroundJobLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulePlanLogEmployees_SchedulePlanLogs_SchedulePlanBackgroundJobLogId",
                schema: "Dawem",
                table: "SchedulePlanLogEmployees",
                column: "SchedulePlanBackgroundJobLogId",
                principalSchema: "Dawem",
                principalTable: "SchedulePlanLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
