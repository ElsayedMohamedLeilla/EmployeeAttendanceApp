using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowFingerprintOutsideAllowedZones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowFingerprintOutsideAllowedZones",
                schema: "Dawem",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AllowFingerprintOutsideAllowedZones",
                schema: "Dawem",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowFingerprintOutsideAllowedZones",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AllowFingerprintOutsideAllowedZones",
                schema: "Dawem",
                table: "Departments");
        }
    }
}
