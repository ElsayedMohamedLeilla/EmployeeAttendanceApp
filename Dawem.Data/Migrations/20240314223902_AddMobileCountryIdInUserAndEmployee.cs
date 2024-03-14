using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMobileCountryIdInUserAndEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyUsers_MobileCountryId",
                schema: "Dawem",
                table: "MyUsers",
                column: "MobileCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_MobileCountryId",
                schema: "Dawem",
                table: "Employees",
                column: "MobileCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Countries_MobileCountryId",
                schema: "Dawem",
                table: "Employees",
                column: "MobileCountryId",
                principalSchema: "Dawem",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyUsers_Countries_MobileCountryId",
                schema: "Dawem",
                table: "MyUsers",
                column: "MobileCountryId",
                principalSchema: "Dawem",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Countries_MobileCountryId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_MyUsers_Countries_MobileCountryId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_MyUsers_MobileCountryId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Employees_MobileCountryId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "InsertedFromExcel",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "Employees");
        }
    }
}
