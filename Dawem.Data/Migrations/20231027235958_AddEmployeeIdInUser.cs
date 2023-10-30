using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeIdInUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyUsers_EmployeeId",
                schema: "Dawem",
                table: "MyUsers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyUsers_Employees_EmployeeId",
                schema: "Dawem",
                table: "MyUsers",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyUsers_Employees_EmployeeId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropIndex(
                name: "IX_MyUsers_EmployeeId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Dawem",
                table: "MyUsers");
        }
    }
}
