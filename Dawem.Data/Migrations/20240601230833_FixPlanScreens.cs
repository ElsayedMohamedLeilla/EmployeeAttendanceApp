using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPlanScreens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanScreens_Languages_LanguageId",
                schema: "Dawem",
                table: "PlanScreens");

            migrationBuilder.DropIndex(
                name: "IX_PlanScreens_LanguageId",
                schema: "Dawem",
                table: "PlanScreens");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                schema: "Dawem",
                table: "PlanScreens");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Dawem",
                table: "PlanScreens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                schema: "Dawem",
                table: "PlanScreens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Dawem",
                table: "PlanScreens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanScreens_LanguageId",
                schema: "Dawem",
                table: "PlanScreens",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanScreens_Languages_LanguageId",
                schema: "Dawem",
                table: "PlanScreens",
                column: "LanguageId",
                principalSchema: "Dawem",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
