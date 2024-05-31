using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScreenIdInPermissionLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from dawem.PermissionLogs");

            migrationBuilder.DropColumn(
                name: "ScreenCode",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.RenameColumn(
                name: "ScreenCode",
                schema: "Dawem",
                table: "PermissionLogs",
                newName: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLogs_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "ScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionLogs_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionLogs_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropIndex(
                name: "IX_PermissionLogs_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.RenameColumn(
                name: "ScreenId",
                schema: "Dawem",
                table: "PermissionLogs",
                newName: "ScreenCode");

            migrationBuilder.AddColumn<int>(
                name: "ScreenCode",
                schema: "Dawem",
                table: "PermissionScreens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
