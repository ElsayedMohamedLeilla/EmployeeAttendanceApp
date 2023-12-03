using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RequestCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefuseReason",
                schema: "Dawem",
                table: "Requests",
                newName: "RejectReason");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "RequestVacations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "RequestTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "RequestPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "RequestJustifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "RequestAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "RequestVacations");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "RequestTasks");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "RequestPermissions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "RequestJustifications");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "RequestAssignments");

            migrationBuilder.RenameColumn(
                name: "RejectReason",
                schema: "Dawem",
                table: "Requests",
                newName: "RefuseReason");
        }
    }
}
