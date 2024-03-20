using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class HandleSubscriptionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionDurationInMonths",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.CreateTable(
                name: "DawemSettings",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupType = table.Column<int>(type: "int", nullable: false),
                    GroupTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ValueType = table.Column<int>(type: "int", nullable: false),
                    ValueTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    String = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Integer = table.Column<int>(type: "int", nullable: true),
                    Decimal = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: true),
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
                    table.PrimaryKey("PK_DawemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MinNumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    MaxNumberOfEmployees = table.Column<int>(type: "int", nullable: false),
                    EmployeeCost = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
                    GracePeriodPercentage = table.Column<int>(type: "int", nullable: false),
                    IsTrial = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RenewalCount = table.Column<int>(type: "int", nullable: false),
                    FollowUpEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "Dawem",
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    LogTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_SubscriptionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionLogs_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalSchema: "Dawem",
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionLogs_SubscriptionId",
                schema: "Dawem",
                table: "SubscriptionLogs",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                schema: "Dawem",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "Subscriptions",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DawemSettings",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SubscriptionLogs",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Subscriptions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Plans",
                schema: "Dawem");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionDurationInMonths",
                schema: "Dawem",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
