using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class convertRadiusFromDecimalToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Radius",
                schema: "Dawem",
                table: "Zones",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Radius",
                schema: "Dawem",
                table: "Zones",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
