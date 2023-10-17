using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeschemadb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserTokens",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserScreenActionPermissions",
                newName: "UserScreenActionPermissions",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserLogIns",
                newName: "UserLogIns",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "UserGroups",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaims",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserBranches",
                newName: "UserBranches",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Translations",
                newName: "Translations",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "RoleClaims",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "MyUsers",
                newName: "MyUsers",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Groups",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currencies",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Countries",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "Companies",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branches",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "ActionLogs",
                newName: "ActionLogs",
                newSchema: "Dawem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "Dawem",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "UserScreenActionPermissions",
                schema: "Dawem",
                newName: "UserScreenActionPermissions");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Dawem",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogIns",
                schema: "Dawem",
                newName: "UserLogIns");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                schema: "Dawem",
                newName: "UserGroups");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "Dawem",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserBranches",
                schema: "Dawem",
                newName: "UserBranches");

            migrationBuilder.RenameTable(
                name: "Translations",
                schema: "Dawem",
                newName: "Translations");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Dawem",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "Dawem",
                newName: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "MyUsers",
                schema: "Dawem",
                newName: "MyUsers");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "Dawem",
                newName: "Groups");

            migrationBuilder.RenameTable(
                name: "Currencies",
                schema: "Dawem",
                newName: "Currencies");

            migrationBuilder.RenameTable(
                name: "Countries",
                schema: "Dawem",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "Companies",
                schema: "Dawem",
                newName: "Companies");

            migrationBuilder.RenameTable(
                name: "Branches",
                schema: "Dawem",
                newName: "Branches");

            migrationBuilder.RenameTable(
                name: "ActionLogs",
                schema: "Dawem",
                newName: "ActionLogs");
        }
    }
}
