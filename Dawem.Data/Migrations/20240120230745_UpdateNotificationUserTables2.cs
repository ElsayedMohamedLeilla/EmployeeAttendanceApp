using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationUserTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationUserDeviceTokenOlds",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "NotificationUserOlds",
                schema: "Dawem");

            migrationBuilder.CreateTable(
                name: "NotificationUsers",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_NotificationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationUsers_MyUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Dawem",
                        principalTable: "MyUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationUserDeviceTokens",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationUserId = table.Column<int>(type: "int", nullable: false),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    LastLogInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificationUserId1 = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_NotificationUserDeviceTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId",
                        column: x => x.NotificationUserId,
                        principalSchema: "Dawem",
                        principalTable: "NotificationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationUserDeviceTokens_NotificationUsers_NotificationUserId1",
                        column: x => x.NotificationUserId1,
                        principalSchema: "Dawem",
                        principalTable: "NotificationUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokens_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokens_NotificationUserId1",
                schema: "Dawem",
                table: "NotificationUserDeviceTokens",
                column: "NotificationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUsers_CompanyId",
                schema: "Dawem",
                table: "NotificationUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUsers_UserId",
                schema: "Dawem",
                table: "NotificationUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationUserDeviceTokens",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "NotificationUsers",
                schema: "Dawem");

            migrationBuilder.CreateTable(
                name: "NotificationUserOlds",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUserOlds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUserOlds_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationUserOlds_MyUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Dawem",
                        principalTable: "MyUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationUserDeviceTokenOlds",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationUserId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastLogInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationUserOldId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationUserDeviceTokenOlds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationUserDeviceTokenOlds_NotificationUserOlds_NotificationUserId",
                        column: x => x.NotificationUserId,
                        principalSchema: "Dawem",
                        principalTable: "NotificationUserOlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationUserDeviceTokenOlds_NotificationUserOlds_NotificationUserOldId",
                        column: x => x.NotificationUserOldId,
                        principalSchema: "Dawem",
                        principalTable: "NotificationUserOlds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokenOlds_NotificationUserId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokenOlds",
                column: "NotificationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserDeviceTokenOlds_NotificationUserOldId",
                schema: "Dawem",
                table: "NotificationUserDeviceTokenOlds",
                column: "NotificationUserOldId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserOlds_CompanyId",
                schema: "Dawem",
                table: "NotificationUserOlds",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUserOlds_UserId",
                schema: "Dawem",
                table: "NotificationUserOlds",
                column: "UserId");
        }
    }
}
