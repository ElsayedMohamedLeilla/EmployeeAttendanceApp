using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateGrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Branches_MainBranchId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "UserGroups",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "NameAr",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                schema: "Dawem",
                table: "Groups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "MainBranchId",
                schema: "Dawem",
                table: "Groups",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_MainBranchId",
                schema: "Dawem",
                table: "Groups",
                newName: "IX_Groups_CompanyId");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                schema: "Dawem",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Companies_CompanyId",
                schema: "Dawem",
                table: "Groups",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Companies_CompanyId",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Dawem",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "Dawem",
                table: "Groups",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "Dawem",
                table: "Groups",
                newName: "MainBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_CompanyId",
                schema: "Dawem",
                table: "Groups",
                newName: "IX_Groups_MainBranchId");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                schema: "Dawem",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_MyUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Dawem",
                        principalTable: "MyUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                schema: "Dawem",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                schema: "Dawem",
                table: "UserGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Branches_MainBranchId",
                schema: "Dawem",
                table: "Groups",
                column: "MainBranchId",
                principalSchema: "Dawem",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
