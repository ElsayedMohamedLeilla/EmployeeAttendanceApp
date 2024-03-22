using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyUsers_Branches_BranchId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_Branches_BranchId",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_UserBranches_BranchId",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropIndex(
                name: "IX_MyUsers_BranchId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "Dawem",
                table: "MyUsers");

            //migrationBuilder.AddColumn<bool>(
            //    name: "InsertedFromExcel",
            //    schema: "Dawem",
            //    table: "Zones",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HeadquarterAddress",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadquarterLocation",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadquarterPostalCode",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ImportDefaultData",
                schema: "Dawem",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoImageName",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreferredLanguageId",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalNumberOfEmployees",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyAttachments",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_CompanyAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAttachments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBranches",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_CompanyBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyBranches_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyIndustries",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CompanyIndustries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyIndustries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NativeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ISO2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ISO3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PreferredLanguageId",
                schema: "Dawem",
                table: "Companies",
                column: "PreferredLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAttachments_CompanyId",
                schema: "Dawem",
                table: "CompanyAttachments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "CompanyBranches",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "CompanyIndustries",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Languages_PreferredLanguageId",
                schema: "Dawem",
                table: "Companies",
                column: "PreferredLanguageId",
                principalSchema: "Dawem",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Languages_PreferredLanguageId",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyAttachments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "CompanyBranches",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "CompanyIndustries",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_Companies_PreferredLanguageId",
                schema: "Dawem",
                table: "Companies");

            //migrationBuilder.DropColumn(
            //    name: "InsertedFromExcel",
            //    schema: "Dawem",
            //    table: "Zones");

            migrationBuilder.DropColumn(
                name: "HeadquarterAddress",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "HeadquarterLocation",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "HeadquarterPostalCode",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ImportDefaultData",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LogoImageName",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "PreferredLanguageId",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TotalNumberOfEmployees",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "WebSite",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    MainBranchId = table.Column<int>(type: "int", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AdminUserId = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsMainBranch = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Branches_MainBranchId",
                        column: x => x.MainBranchId,
                        principalSchema: "Dawem",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Branches_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Dawem",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_BranchId",
                schema: "Dawem",
                table: "UserBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MyUsers_BranchId",
                schema: "Dawem",
                table: "MyUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CountryId",
                schema: "Dawem",
                table: "Branches",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_MainBranchId",
                schema: "Dawem",
                table: "Branches",
                column: "MainBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Name_IsDeleted",
                schema: "Dawem",
                table: "Branches",
                columns: new[] { "CompanyId", "Name", "IsDeleted" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MyUsers_Branches_BranchId",
                schema: "Dawem",
                table: "MyUsers",
                column: "BranchId",
                principalSchema: "Dawem",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_Branches_BranchId",
                schema: "Dawem",
                table: "UserBranches",
                column: "BranchId",
                principalSchema: "Dawem",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
