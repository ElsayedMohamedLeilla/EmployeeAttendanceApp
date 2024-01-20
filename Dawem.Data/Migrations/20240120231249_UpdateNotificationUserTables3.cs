using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationUserTables3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

            migrationBuilder.DropIndex(
                name: "IX_NotificationUserDeviceTokens_NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

            migrationBuilder.DropColumn(
                name: "NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokens_NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId1",
                principalSchema: "Dawem",
                principalTable: "NotificationUsers",
                principalColumn: "Id");
        }
    }
}
