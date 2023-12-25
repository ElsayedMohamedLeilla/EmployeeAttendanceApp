using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AmgadAndElsayedMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                schema: "Dawem",
                table: "MyUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            //migrationBuilder.AddColumn<int>(
            //    name: "ManagerId",
            //    schema: "Dawem",
            //    table: "Departments",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "DepartmentManagerDelegators",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EmployeeId = table.Column<int>(type: "int", nullable: false),
            //        DepartmentId = table.Column<int>(type: "int", nullable: true),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        AddedApplicationType = table.Column<int>(type: "int", nullable: false),
            //        ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
            //        AddUserId = table.Column<int>(type: "int", nullable: true),
            //        ModifyUserId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DepartmentManagerDelegators", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_DepartmentManagerDelegators_Departments_DepartmentId",
            //            column: x => x.DepartmentId,
            //            principalSchema: "Dawem",
            //            principalTable: "Departments",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_DepartmentManagerDelegators_Employees_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalSchema: "Dawem",
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Zones",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyId = table.Column<int>(type: "int", nullable: false),
            //        Code = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Latitude = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
            //        Longitude = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
            //        Radius = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: true),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        AddedApplicationType = table.Column<int>(type: "int", nullable: false),
            //        ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
            //        AddUserId = table.Column<int>(type: "int", nullable: true),
            //        ModifyUserId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DisableReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Zones", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Zones_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalSchema: "Dawem",
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "ZoneDepartments",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ZoneDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Dawem",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZoneDepartments_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZoneEmployees",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ZoneEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZoneEmployees_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZoneGroups",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ZoneGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Dawem",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZoneGroups_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Dawem",
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Departments_ManagerId",
            //    schema: "Dawem",
            //    table: "Departments",
            //    column: "ManagerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DepartmentManagerDelegators_DepartmentId",
            //    schema: "Dawem",
            //    table: "DepartmentManagerDelegators",
            //    column: "DepartmentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DepartmentManagerDelegators_EmployeeId",
            //    schema: "Dawem",
            //    table: "DepartmentManagerDelegators",
            //    column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneDepartments_DepartmentId",
                schema: "Dawem",
                table: "ZoneDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneDepartments_ZoneId",
                schema: "Dawem",
                table: "ZoneDepartments",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneEmployees_EmployeeId",
                schema: "Dawem",
                table: "ZoneEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneEmployees_ZoneId",
                schema: "Dawem",
                table: "ZoneEmployees",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneGroups_GroupId",
                schema: "Dawem",
                table: "ZoneGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneGroups_ZoneId",
                schema: "Dawem",
                table: "ZoneGroups",
                column: "ZoneId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Zones_CompanyId",
            //    schema: "Dawem",
            //    table: "Zones",
            //    column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Dawem",
                table: "Departments",
                column: "ManagerId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "DepartmentManagerDelegators",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneDepartments",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneEmployees",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "ZoneGroups",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "Zones",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ManagerId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "Dawem",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                schema: "Dawem",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "Dawem",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
