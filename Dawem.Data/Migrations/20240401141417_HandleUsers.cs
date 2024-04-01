using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "Permissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "PermissionLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "PermissionLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Permissions",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true,
                filter: "[CompanyId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsForAdminPanel",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "PermissionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Permissions",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }
    }
}
