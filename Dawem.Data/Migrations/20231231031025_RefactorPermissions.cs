using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScreenPermissionLogs",
                schema: "Dawem");

            migrationBuilder.RenameColumn(
                name: "Action",
                schema: "Dawem",
                table: "PermissionScreenActions",
                newName: "ActionCode");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PermissionLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    ScreenCode = table.Column<int>(type: "int", nullable: false),
                    ActionCode = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionLogs_Companies_UserId",
                        column: x => x.UserId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLogs_CompanyId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLogs_UserId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionLogs",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "Permissions");

            migrationBuilder.RenameColumn(
                name: "ActionCode",
                schema: "Dawem",
                table: "PermissionScreenActions",
                newName: "Action");

            migrationBuilder.CreateTable(
                name: "ScreenPermissionLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseStatus = table.Column<int>(type: "int", nullable: false),
                    ScreenCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenPermissionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenPermissionLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScreenPermissionLogs_Companies_UserId",
                        column: x => x.UserId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScreenPermissionLogs_CompanyId",
                schema: "Dawem",
                table: "ScreenPermissionLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenPermissionLogs_UserId",
                schema: "Dawem",
                table: "ScreenPermissionLogs",
                column: "UserId");
        }
    }
}
