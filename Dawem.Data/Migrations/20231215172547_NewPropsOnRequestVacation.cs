using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewPropsOnRequestVacation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "BalanceAfterRequest",
                schema: "Dawem",
                table: "RequestVacations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "BalanceBeforeRequest",
                schema: "Dawem",
                table: "RequestVacations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDays",
                schema: "Dawem",
                table: "RequestVacations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAfterRequest",
                schema: "Dawem",
                table: "RequestVacations");

            migrationBuilder.DropColumn(
                name: "BalanceBeforeRequest",
                schema: "Dawem",
                table: "RequestVacations");

            migrationBuilder.DropColumn(
                name: "NumberOfDays",
                schema: "Dawem",
                table: "RequestVacations");
        }
    }
}
