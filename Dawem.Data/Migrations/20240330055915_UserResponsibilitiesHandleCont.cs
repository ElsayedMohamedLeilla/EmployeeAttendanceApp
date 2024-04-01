using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserResponsibilitiesHandleCont : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponsibilities_MyUsers_UserId",
                schema: "Dawem",
                table: "UserResponsibilities");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                schema: "Dawem",
                table: "Permissions",
                newName: "ResponsibilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_RoleId",
                schema: "Dawem",
                table: "Permissions",
                newName: "IX_Permissions_ResponsibilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Responsibilities_ResponsibilityId",
                schema: "Dawem",
                table: "Permissions",
                column: "ResponsibilityId",
                principalSchema: "Dawem",
                principalTable: "Responsibilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponsibilities_MyUsers_UserId",
                schema: "Dawem",
                table: "UserResponsibilities",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Responsibilities_ResponsibilityId",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserResponsibilities_MyUsers_UserId",
                schema: "Dawem",
                table: "UserResponsibilities");

            migrationBuilder.RenameColumn(
                name: "ResponsibilityId",
                schema: "Dawem",
                table: "Permissions",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_ResponsibilityId",
                schema: "Dawem",
                table: "Permissions",
                newName: "IX_Permissions_RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                schema: "Dawem",
                table: "Permissions",
                column: "RoleId",
                principalSchema: "Dawem",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserResponsibilities_MyUsers_UserId",
                schema: "Dawem",
                table: "UserResponsibilities",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
