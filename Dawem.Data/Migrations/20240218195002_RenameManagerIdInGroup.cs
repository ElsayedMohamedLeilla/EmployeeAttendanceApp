using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameManagerIdInGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Employees_GroupManagerId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "GroupManagerId",
                schema: "Dawem",
                table: "Groups",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_GroupManagerId",
                schema: "Dawem",
                table: "Groups",
                newName: "IX_Groups_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Employees_ManagerId",
                schema: "Dawem",
                table: "Groups",
                column: "ManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Employees_ManagerId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                schema: "Dawem",
                table: "Groups",
                newName: "GroupManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_ManagerId",
                schema: "Dawem",
                table: "Groups",
                newName: "IX_Groups_GroupManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Employees_GroupManagerId",
                schema: "Dawem",
                table: "Groups",
                column: "GroupManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
