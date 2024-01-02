using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "Dawem",
                table: "VacationTypes",
                newName: "DefaultType");

            migrationBuilder.RenameColumn(
                name: "VacationType",
                schema: "Dawem",
                table: "VacationBalances",
                newName: "DefaultVacationType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DefaultType",
                schema: "Dawem",
                table: "VacationTypes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "DefaultVacationType",
                schema: "Dawem",
                table: "VacationBalances",
                newName: "VacationType");
        }
    }
}
