using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjastTableUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "Dawem",
                table: "MyUsers",
                newName: "ProfileImageName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "Dawem",
                table: "MyUsers",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Dawem",
                table: "MyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.RenameColumn(
                name: "ProfileImageName",
                schema: "Dawem",
                table: "MyUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Dawem",
                table: "MyUsers",
                newName: "FirstName");
        }
    }
}
