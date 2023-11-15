using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_GroupId",
                schema: "Dawem",
                table: "Employees",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Groups_GroupId",
                schema: "Dawem",
                table: "Employees",
                column: "GroupId",
                principalSchema: "Dawem",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Groups_GroupId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_GroupId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "Dawem",
                table: "Employees");
        }
    }
}
