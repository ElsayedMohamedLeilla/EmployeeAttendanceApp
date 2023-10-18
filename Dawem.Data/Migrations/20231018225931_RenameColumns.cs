using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                schema: "Dawem",
                table: "Companies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BranchName",
                schema: "Dawem",
                table: "Branches",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Dawem",
                table: "Companies",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Dawem",
                table: "Branches",
                newName: "BranchName");
        }
    }
}
