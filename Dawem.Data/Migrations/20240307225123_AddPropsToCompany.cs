using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropsToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfEmployees",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update Dawem.Companies set NumberOfEmployees = 100");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionDurationInMonths",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update Dawem.Companies set SubscriptionDurationInMonths = 6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfEmployees",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SubscriptionDurationInMonths",
                schema: "Dawem",
                table: "Companies");
        }
    }
}
