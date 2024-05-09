using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullMessage",
                schema: "Dawem",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "ShortMessege",
                schema: "Dawem",
                table: "Notifications",
                newName: "NotificationTypeName");

            migrationBuilder.CreateTable(
                name: "NotificationTranslations",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_NotificationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Dawem",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationTranslations_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "Dawem",
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTranslations_LanguageId",
                schema: "Dawem",
                table: "NotificationTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTranslations_NotificationId",
                schema: "Dawem",
                table: "NotificationTranslations",
                column: "NotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationTranslations",
                schema: "Dawem");

            migrationBuilder.RenameColumn(
                name: "NotificationTypeName",
                schema: "Dawem",
                table: "Notifications",
                newName: "ShortMessege");

            migrationBuilder.AddColumn<string>(
                name: "FullMessage",
                schema: "Dawem",
                table: "Notifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
