using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class departmentAddcolumnID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Dawem",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                schema: "Dawem",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                schema: "Dawem",
                table: "Departments",
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
                name: "FK_Departments_Companies_CompanyId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_CompanyId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Dawem",
                table: "Departments");
        }
    }
}
