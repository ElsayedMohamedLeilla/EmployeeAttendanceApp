using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubscritionsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWaitingForApproval",
                schema: "Dawem",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CompanyId",
                schema: "Dawem",
                table: "Subscriptions",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_CompanyId",
                schema: "Dawem",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsWaitingForApproval",
                schema: "Dawem",
                table: "Subscriptions");
        }
    }
}
