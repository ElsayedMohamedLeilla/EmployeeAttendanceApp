using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class Responsibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responsibilitys_Companies_CompanyId",
                schema: "Dawem",
                table: "Responsibilitys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responsibilitys",
                schema: "Dawem",
                table: "Responsibilitys");

            migrationBuilder.RenameTable(
                name: "Responsibilitys",
                schema: "Dawem",
                newName: "Responsibilities",
                newSchema: "Dawem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responsibilities",
                schema: "Dawem",
                table: "Responsibilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responsibilities_Companies_CompanyId",
                schema: "Dawem",
                table: "Responsibilities",
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
                name: "FK_Responsibilities_Companies_CompanyId",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responsibilities",
                schema: "Dawem",
                table: "Responsibilities");

            migrationBuilder.RenameTable(
                name: "Responsibilities",
                schema: "Dawem",
                newName: "Responsibilitys",
                newSchema: "Dawem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responsibilitys",
                schema: "Dawem",
                table: "Responsibilitys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responsibilitys_Companies_CompanyId",
                schema: "Dawem",
                table: "Responsibilitys",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
