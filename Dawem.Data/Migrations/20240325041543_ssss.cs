using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "Dawem",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "NameEn",
                schema: "Dawem",
                table: "Plans");

            migrationBuilder.CreateTable(
                name: "PlanNameTranslations",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanNameTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanNameTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Dawem",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanNameTranslations_Plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "Dawem",
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanNameTranslations_LanguageId",
                schema: "Dawem",
                table: "PlanNameTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanNameTranslations_PlanId",
                schema: "Dawem",
                table: "PlanNameTranslations",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanNameTranslations",
                schema: "Dawem");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "Dawem",
                table: "Plans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                schema: "Dawem",
                table: "Plans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
