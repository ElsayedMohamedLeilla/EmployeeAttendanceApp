using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationUserTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_FirebaseUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

            migrationBuilder.DropIndex(
                name: "IX_NotificationUserDeviceTokens_FirebaseUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

            migrationBuilder.DropColumn(
                name: "FirebaseUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId",
                principalSchema: "Dawem",
                principalTable: "NotificationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId1",
                principalSchema: "Dawem",
                principalTable: "NotificationUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens");

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

            migrationBuilder.AlterColumn<int>(
                name: "NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FirebaseUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokens_FirebaseUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "FirebaseUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_FirebaseUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "FirebaseUserId",
                principalSchema: "Dawem",
                principalTable: "NotificationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId",
                principalSchema: "Dawem",
                principalTable: "NotificationUsers",
                principalColumn: "Id");
        }
    }
}
