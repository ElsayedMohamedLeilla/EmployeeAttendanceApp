using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSpecifiedByYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidaies_Companies_CompanyId",
                schema: "Dawem",
                table: "Holidaies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holidaies",
                schema: "Dawem",
                table: "Holidaies");

            migrationBuilder.RenameTable(
                name: "Holidaies",
                schema: "Dawem",
                newName: "Holidays",
                newSchema: "Dawem");

            migrationBuilder.RenameIndex(
                name: "IX_Holidaies_CompanyId",
                schema: "Dawem",
                table: "Holidays",
                newName: "IX_Holidays_CompanyId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecifiedByYear",
                schema: "Dawem",
                table: "Holidays",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holidays",
                schema: "Dawem",
                table: "Holidays",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Companies_CompanyId",
                schema: "Dawem",
                table: "Holidays",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Companies_CompanyId",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Holidays",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "IsSpecifiedByYear",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.RenameTable(
                name: "Holidays",
                schema: "Dawem",
                newName: "Holidaies",
                newSchema: "Dawem");

            migrationBuilder.RenameIndex(
                name: "IX_Holidays_CompanyId",
                schema: "Dawem",
                table: "Holidaies",
                newName: "IX_Holidaies_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Holidaies",
                schema: "Dawem",
                table: "Holidaies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Holidaies_Companies_CompanyId",
                schema: "Dawem",
                table: "Holidaies",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
