using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesInPermissionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionLogs_Companies_UserId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionLogs_MyUsers_UserId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionLogs_MyUsers_UserId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionLogs_Companies_UserId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
