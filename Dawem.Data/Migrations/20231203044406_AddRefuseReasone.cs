using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRefuseReasone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefuseReason",
                schema: "Dawem",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefuseReason",
                schema: "Dawem",
                table: "Requests");
        }
    }
}
