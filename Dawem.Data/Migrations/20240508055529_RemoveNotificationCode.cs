using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotificationCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CompanyId",
                schema: "Dawem",
                table: "Notifications",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_CompanyId",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Notifications",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }
    }
}
