using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "Dawem",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                schema: "Dawem",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Departments_ParentId",
                schema: "Dawem",
                table: "Departments",
                column: "ParentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Departments_ParentId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ParentId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "Dawem",
                table: "Departments");
        }
    }
}
