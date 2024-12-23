using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OvertimeTypes",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_OvertimeTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OvertimeTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestOvertimes",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    OvertimeTypeId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OvertimeHoursCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RequestOvertimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestOvertimes_OvertimeTypes_OvertimeTypeId",
                        column: x => x.OvertimeTypeId,
                        principalSchema: "Dawem",
                        principalTable: "OvertimeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestOvertimes_Requests_RequestId",
                        column: x => x.RequestId,
                        principalSchema: "Dawem",
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "OvertimeTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Non_Unique_Name",
                schema: "Dawem",
                table: "OvertimeTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "OvertimeTypes",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "OvertimeTypes",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "RequestOvertimes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RequestOvertimes_OvertimeTypeId",
                schema: "Dawem",
                table: "RequestOvertimes",
                column: "OvertimeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestOvertimes_RequestId",
                schema: "Dawem",
                table: "RequestOvertimes",
                column: "RequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestOvertimes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "OvertimeTypes",
                schema: "Dawem");
        }
    }
}
