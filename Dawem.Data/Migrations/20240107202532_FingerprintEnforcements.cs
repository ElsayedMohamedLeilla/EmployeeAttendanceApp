using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FingerprintEnforcements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "IconUrl",
            //    schema: "Dawem",
            //    table: "NotificationStores",
            //    newName: "ImageUrl");

            migrationBuilder.CreateTable(
                name: "FingerprintEnforcements",
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
                    Code = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
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
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
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
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
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
                name: "FingerprintEnforcementActions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    FingerprintEnforcementId = table.Column<int>(type: "int", nullable: false),
                    NonComplianceActionId = table.Column<int>(type: "int", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "NonComplianceActions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "FingerprintEnforcements",
                schema: "Dawem");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "Dawem",
                table: "NotificationStores",
                newName: "IconUrl");
        }
    }
}
