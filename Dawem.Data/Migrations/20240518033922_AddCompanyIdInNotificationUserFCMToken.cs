using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyIdInNotificationUserFCMToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserFCMTokens_CompanyId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationUserFCMTokens_Companies_CompanyId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationUserFCMTokens_Companies_CompanyId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens");

            migrationBuilder.DropIndex(
                name: "IX_NotificationUserFCMTokens_CompanyId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Dawem",
                table: "NotificationUserFCMTokens");
        }
    }
}
