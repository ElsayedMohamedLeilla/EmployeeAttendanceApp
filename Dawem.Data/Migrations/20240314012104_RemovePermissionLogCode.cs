using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePermissionLogCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLogs_CompanyId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PermissionLogs_CompanyId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "PermissionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "PermissionLogs",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }
    }
}
