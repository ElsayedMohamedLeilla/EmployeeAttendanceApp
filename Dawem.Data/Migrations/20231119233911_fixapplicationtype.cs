using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixapplicationtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "GroupManagerDelegators",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "GroupManagerDelegators",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedApplicationType",
                schema: "Dawem",
                table: "GroupManagerDelegators");

            migrationBuilder.DropColumn(
                name: "ModifiedApplicationType",
                schema: "Dawem",
                table: "GroupManagerDelegators");
        }
    }
}
