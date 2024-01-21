using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "Dawem",
                table: "NonComplianceActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WarningMessage",
                schema: "Dawem",
                table: "NonComplianceActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcementNotifyWays",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    NotifyWay = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_FingerprintEnforcementNotifyWays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementNotifyWays_Companies_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementNotifyWays_FingerprintEnforcements_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "FingerprintEnforcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementNotifyWays_FingerprintEnforcementId",
                schema: "Dawem",
                table: "FingerprintEnforcementNotifyWays",
                column: "FingerprintEnforcementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FingerprintEnforcementNotifyWays",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Dawem",
                table: "NonComplianceActions");

            migrationBuilder.DropColumn(
                name: "WarningMessage",
                schema: "Dawem",
                table: "NonComplianceActions");
        }
    }
}
