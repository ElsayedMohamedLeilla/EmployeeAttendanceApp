using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScreenGroups2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScreenURL",
                schema: "Dawem",
                table: "Screens",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "ScreenIcon",
                schema: "Dawem",
                table: "Screens",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "GroupIcon",
                schema: "Dawem",
                table: "ScreenGroups",
                newName: "Icon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "URL",
                schema: "Dawem",
                table: "Screens",
                newName: "ScreenURL");

            migrationBuilder.RenameColumn(
                name: "Icon",
                schema: "Dawem",
                table: "Screens",
                newName: "ScreenIcon");

            migrationBuilder.RenameColumn(
                name: "Icon",
                schema: "Dawem",
                table: "ScreenGroups",
                newName: "GroupIcon");
        }
    }
}
