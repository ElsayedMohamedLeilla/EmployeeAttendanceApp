using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorScreen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionLogs_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionScreens_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanScreens_Screens_ScreenId",
                schema: "Dawem",
                table: "PlanScreens");

            migrationBuilder.DropTable(
                name: "ScreenActions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ScreenGroupNameTranslations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ScreenNameTranslations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Screens",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ScreenGroups",
                schema: "Dawem");

            migrationBuilder.CreateTable(
                name: "MenuItems",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    MenuItemCode = table.Column<int>(type: "int", nullable: true),
                    MenuItemCodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupOrScreenType = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    AuthenticationType = table.Column<int>(type: "int", nullable: false),
                    AuthenticationTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuItems_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Dawem",
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemActions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    ActionCode = table.Column<int>(type: "int", nullable: false),
                    ActionCodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_MenuItemActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemActions_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalSchema: "Dawem",
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemNameTranslations",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MenuItemNameTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemNameTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Dawem",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuItemNameTranslations_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalSchema: "Dawem",
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "MenuItemActions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemActions_MenuItemId",
                schema: "Dawem",
                table: "MenuItemActions",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "MenuItemNameTranslations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemNameTranslations_LanguageId",
                schema: "Dawem",
                table: "MenuItemNameTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemNameTranslations_MenuItemId",
                schema: "Dawem",
                table: "MenuItemNameTranslations",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "MenuItems",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_ParentId",
                schema: "Dawem",
                table: "MenuItems",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionLogs_MenuItems_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionScreens_MenuItems_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanScreens_MenuItems_ScreenId",
                schema: "Dawem",
                table: "PlanScreens",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionLogs_MenuItems_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionScreens_MenuItems_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanScreens_MenuItems_ScreenId",
                schema: "Dawem",
                table: "PlanScreens");

            migrationBuilder.DropTable(
                name: "MenuItemActions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "MenuItemNameTranslations",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "MenuItems",
                schema: "Dawem");

            migrationBuilder.CreateTable(
                name: "ScreenGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupType = table.Column<int>(type: "int", nullable: false),
                    GroupTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
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
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ScreenGroupId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Screens",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenGroupId = table.Column<int>(type: "int", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ScreenCode = table.Column<int>(type: "int", nullable: false),
                    ScreenCodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screens_ScreenGroups_ScreenGroupId",
                        column: x => x.ScreenGroupId,
                        principalSchema: "Dawem",
                        principalTable: "ScreenGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScreenActions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScreenId = table.Column<int>(type: "int", nullable: false),
                    ActionCode = table.Column<int>(type: "int", nullable: false),
                    ActionCodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenActions_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalSchema: "Dawem",
                        principalTable: "Screens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScreenNameTranslations",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    ScreenId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenNameTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScreenNameTranslations_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Dawem",
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScreenNameTranslations_Screens_ScreenId",
                        column: x => x.ScreenId,
                        principalSchema: "Dawem",
                        principalTable: "Screens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ScreenActions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenActions_ScreenId",
                schema: "Dawem",
                table: "ScreenActions",
                column: "ScreenId");

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

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "ScreenNameTranslations",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenNameTranslations_LanguageId",
                schema: "Dawem",
                table: "ScreenNameTranslations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenNameTranslations_ScreenId",
                schema: "Dawem",
                table: "ScreenNameTranslations",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_IsDeleted",
                schema: "Dawem",
                table: "Screens",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Screens_ScreenGroupId",
                schema: "Dawem",
                table: "Screens",
                column: "ScreenGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionLogs_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionLogs",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionScreens_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanScreens_Screens_ScreenId",
                schema: "Dawem",
                table: "PlanScreens",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
