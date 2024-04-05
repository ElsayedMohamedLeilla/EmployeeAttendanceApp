using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorisadminpanel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropColumn(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropColumn(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Dawem",
                table: "Responsibilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Dawem",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Dawem",
                table: "PermissionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "Responsibilities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "PermissionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "MyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
