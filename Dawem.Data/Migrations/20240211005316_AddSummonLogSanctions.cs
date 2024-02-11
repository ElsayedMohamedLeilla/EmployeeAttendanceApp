using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSummonLogSanctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoneNotify",
                schema: "Dawem",
                table: "SummonMissingLogs");

            migrationBuilder.CreateTable(
                name: "SummonMissingLogSanctions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SummonMissingLogId = table.Column<int>(type: "int", nullable: false),
                    SummonSanctionId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Done = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_SummonMissingLogSanctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogSanctions_SummonMissingLogs_SummonMissingLogId",
                        column: x => x.SummonMissingLogId,
                        principalSchema: "Dawem",
                        principalTable: "SummonMissingLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogSanctions_SummonSanctions_SummonSanctionId",
                        column: x => x.SummonSanctionId,
                        principalSchema: "Dawem",
                        principalTable: "SummonSanctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogSanctions_SummonMissingLogId",
                schema: "Dawem",
                table: "SummonMissingLogSanctions",
                column: "SummonMissingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogSanctions_SummonSanctionId",
                schema: "Dawem",
                table: "SummonMissingLogSanctions",
                column: "SummonSanctionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SummonMissingLogSanctions",
                schema: "Dawem");

            migrationBuilder.AddColumn<bool>(
                name: "DoneNotify",
                schema: "Dawem",
                table: "SummonMissingLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
