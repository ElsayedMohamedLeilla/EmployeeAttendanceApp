using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSchedualPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchedulePlans",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    SchedulePlanType = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlans_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlans_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "Dawem",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePlanDepartments",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulePlanId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePlanDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlanDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlanDepartments_SchedulePlans_SchedulePlanId",
                        column: x => x.SchedulePlanId,
                        principalSchema: "Dawem",
                        principalTable: "SchedulePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePlanEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulePlanId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePlanEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlanEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlanEmployees_SchedulePlans_SchedulePlanId",
                        column: x => x.SchedulePlanId,
                        principalSchema: "Dawem",
                        principalTable: "SchedulePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchedulePlanGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchedulePlanId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulePlanGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchedulePlanGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchedulePlanGroups_SchedulePlans_SchedulePlanId",
                        column: x => x.SchedulePlanId,
                        principalSchema: "Dawem",
                        principalTable: "SchedulePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanDepartments_DepartmentId",
                schema: "Dawem",
                table: "SchedulePlanDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanDepartments_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanDepartments",
                column: "SchedulePlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanEmployees_EmployeeId",
                schema: "Dawem",
                table: "SchedulePlanEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanEmployees_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanEmployees",
                column: "SchedulePlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanGroups_GroupId",
                schema: "Dawem",
                table: "SchedulePlanGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlanGroups_SchedulePlanId",
                schema: "Dawem",
                table: "SchedulePlanGroups",
                column: "SchedulePlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlans_CompanyId",
                schema: "Dawem",
                table: "SchedulePlans",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedulePlans_ScheduleId",
                schema: "Dawem",
                table: "SchedulePlans",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchedulePlanDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlanGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SchedulePlans",
                schema: "Dawem");
        }
    }
}
