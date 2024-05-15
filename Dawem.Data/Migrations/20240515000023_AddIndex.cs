using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Summons_EndDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons",
                column: "EndDateAndTimeUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Summons_StartDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons",
                column: "StartDateAndTimeUTC");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HelperId",
                schema: "Dawem",
                table: "Notifications",
                column: "HelperId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationType",
                schema: "Dawem",
                table: "Notifications",
                column: "NotificationType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Summons_EndDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropIndex(
                name: "IX_Summons_StartDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_HelperId",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationType",
                schema: "Dawem",
                table: "Notifications");
        }
    }
}
