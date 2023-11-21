using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class hh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupManagerId",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupManagerDelegators",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_GroupManagerDelegators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupManagerDelegators_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupManagerDelegators_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupManagerId",
                schema: "Dawem",
                table: "Groups",
                column: "GroupManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupManagerDelegators_EmployeeId",
                schema: "Dawem",
                table: "GroupManagerDelegators",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupManagerDelegators_GroupId",
                schema: "Dawem",
                table: "GroupManagerDelegators",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Employees_GroupManagerId",
                schema: "Dawem",
                table: "Groups",
                column: "GroupManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Employees_GroupManagerId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupManagerDelegators",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupManagerId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupManagerId",
                schema: "Dawem",
                table: "Groups");
        }
    }
}
