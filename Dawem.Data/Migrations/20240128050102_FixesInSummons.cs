using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixesInSummons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FingerprintEnforcementActions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintEnforcementDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintEnforcementEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintEnforcementGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintEnforcementNotifyWays",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "NonComplianceActions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintEnforcements",
                schema: "Dawem");

            migrationBuilder.CreateTable(
                name: "Sanctions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarningMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Sanctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sanctions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Summons",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    ForType = table.Column<int>(type: "int", nullable: false),
                    ForAllEmployees = table.Column<bool>(type: "bit", nullable: true),
                    FingerprintDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllowedTime = table.Column<int>(type: "int", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Summons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summons_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonDepartments",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SummonDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonDepartments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SummonDepartments_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SummonEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonEmployees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SummonEmployees_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SummonGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonGroups_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SummonGroups_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonNotifyWays",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SummonNotifyWays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonNotifyWays_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonNotifyWays_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonSanctions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    SanctionId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SummonSanctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonSanctions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonSanctions_Sanctions_SanctionId",
                        column: x => x.SanctionId,
                        principalSchema: "Dawem",
                        principalTable: "Sanctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonSanctions_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sanctions_CompanyId",
                schema: "Dawem",
                table: "Sanctions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonDepartments_CompanyId",
                schema: "Dawem",
                table: "SummonDepartments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonDepartments_DepartmentId",
                schema: "Dawem",
                table: "SummonDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonDepartments_SummonId",
                schema: "Dawem",
                table: "SummonDepartments",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonEmployees_CompanyId",
                schema: "Dawem",
                table: "SummonEmployees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonEmployees_EmployeeId",
                schema: "Dawem",
                table: "SummonEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonEmployees_SummonId",
                schema: "Dawem",
                table: "SummonEmployees",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonGroups_CompanyId",
                schema: "Dawem",
                table: "SummonGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonGroups_GroupId",
                schema: "Dawem",
                table: "SummonGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonGroups_SummonId",
                schema: "Dawem",
                table: "SummonGroups",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonNotifyWays_CompanyId",
                schema: "Dawem",
                table: "SummonNotifyWays",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonNotifyWays_SummonId",
                schema: "Dawem",
                table: "SummonNotifyWays",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_Summons_CompanyId",
                schema: "Dawem",
                table: "Summons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonSanctions_CompanyId",
                schema: "Dawem",
                table: "SummonSanctions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonSanctions_SanctionId",
                schema: "Dawem",
                table: "SummonSanctions",
                column: "SanctionId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonSanctions_SummonId",
                schema: "Dawem",
                table: "SummonSanctions",
                column: "SummonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SummonDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonNotifyWays",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonSanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Sanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Summons",
                schema: "Dawem");

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcements",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllowedTime = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FingerprintDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForAllEmployees = table.Column<bool>(type: "bit", nullable: true),
                    ForType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerprintEnforcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcements_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NonComplianceActions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WarningMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonComplianceActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonComplianceActions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcementDepartments",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FingerprintEnforcementDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementDepartments_Companies_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementDepartments_FingerprintEnforcements_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "FingerprintEnforcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcementEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FingerprintEnforcementEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementEmployees_Companies_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementEmployees_FingerprintEnforcements_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "FingerprintEnforcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcementGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FingerprintEnforcementGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementGroups_Companies_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementGroups_FingerprintEnforcements_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "FingerprintEnforcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcementNotifyWays",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotifyWay = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcementActions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    NonComplianceActionId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerprintEnforcementActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementActions_Companies_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementActions_FingerprintEnforcements_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "FingerprintEnforcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FingerprintEnforcementActions_NonComplianceActions_FingerprintEnforcementId",
                        column: x => x.FingerprintEnforcementId,
                        principalSchema: "Dawem",
                        principalTable: "NonComplianceActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementActions_FingerprintEnforcementId",
                schema: "Dawem",
                table: "FingerprintEnforcementActions",
                column: "FingerprintEnforcementId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementDepartments_DepartmentId",
                schema: "Dawem",
                table: "FingerprintEnforcementDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementDepartments_FingerprintEnforcementId",
                schema: "Dawem",
                table: "FingerprintEnforcementDepartments",
                column: "FingerprintEnforcementId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementEmployees_EmployeeId",
                schema: "Dawem",
                table: "FingerprintEnforcementEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementEmployees_FingerprintEnforcementId",
                schema: "Dawem",
                table: "FingerprintEnforcementEmployees",
                column: "FingerprintEnforcementId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementGroups_FingerprintEnforcementId",
                schema: "Dawem",
                table: "FingerprintEnforcementGroups",
                column: "FingerprintEnforcementId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementGroups_GroupId",
                schema: "Dawem",
                table: "FingerprintEnforcementGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcementNotifyWays_FingerprintEnforcementId",
                schema: "Dawem",
                table: "FingerprintEnforcementNotifyWays",
                column: "FingerprintEnforcementId");

            migrationBuilder.CreateIndex(
                name: "IX_FingerprintEnforcements_CompanyId",
                schema: "Dawem",
                table: "FingerprintEnforcements",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_NonComplianceActions_CompanyId",
                schema: "Dawem",
                table: "NonComplianceActions",
                column: "CompanyId");
        }
    }
}
