using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ScreenGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "Dawem",
                table: "Screens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ScreenGroupId",
                schema: "Dawem",
                table: "Screens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenIcon",
                schema: "Dawem",
                table: "Screens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenURL",
                schema: "Dawem",
                table: "Screens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllScreensAvailable",
                schema: "Dawem",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ScreenGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupType = table.Column<int>(type: "int", nullable: false),
                    GroupTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupIcon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
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
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScreenGroupNameTranslations",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenGroupId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ScreenGroupNameTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenGroupNameTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Dawem",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScreenGroupNameTranslations_ScreenGroups_ScreenGroupId",
                        column: x => x.ScreenGroupId,
                        principalSchema: "Dawem",
                        principalTable: "ScreenGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Screens_ScreenGroupId",
                schema: "Dawem",
                table: "Screens",
                column: "ScreenGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ScreenGroupNameTranslations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenGroupNameTranslations_LanguageId",
                schema: "Dawem",
                table: "ScreenGroupNameTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenGroupNameTranslations_ScreenGroupId",
                schema: "Dawem",
                table: "ScreenGroupNameTranslations",
                column: "ScreenGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ScreenGroups",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Screens_ScreenGroups_ScreenGroupId",
                schema: "Dawem",
                table: "Screens",
                column: "ScreenGroupId",
                principalSchema: "Dawem",
                principalTable: "ScreenGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Screens_ScreenGroups_ScreenGroupId",
                schema: "Dawem",
                table: "Screens");

            migrationBuilder.DropTable(
                name: "ScreenGroupNameTranslations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ScreenGroups",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_Screens_ScreenGroupId",
                schema: "Dawem",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "Dawem",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "ScreenGroupId",
                schema: "Dawem",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "ScreenIcon",
                schema: "Dawem",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "ScreenURL",
                schema: "Dawem",
                table: "Screens");

            migrationBuilder.DropColumn(
                name: "AllScreensAvailable",
                schema: "Dawem",
                table: "Plans");
        }
    }
}
