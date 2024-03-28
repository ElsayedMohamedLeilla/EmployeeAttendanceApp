using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSummons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("update Dawem.EmployeeAttendanceChecks set SummonId = null");

            migrationBuilder.DropTable(
                name: "SummonMissingLogSanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonMissingLogs",
                schema: "Dawem");

            migrationBuilder.RenameColumn(
                name: "DateAndTime",
                schema: "Dawem",
                table: "Summons",
                newName: "StartDateAndTimeUTC");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LocalDateAndTime",
                schema: "Dawem",
                table: "Summons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SummonLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    DoneSummon = table.Column<bool>(type: "bit", nullable: false),
                    DoneTakeActions = table.Column<bool>(type: "bit", nullable: false),
                    DoneDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_SummonLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonLogs_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonLogSanctions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SummonLogId = table.Column<int>(type: "int", nullable: false),
                    SummonSanctionId = table.Column<int>(type: "int", nullable: false),
                    Done = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_SummonLogSanctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonLogSanctions_SummonLogs_SummonLogId",
                        column: x => x.SummonLogId,
                        principalSchema: "Dawem",
                        principalTable: "SummonLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SummonLogSanctions_SummonSanctions_SummonSanctionId",
                        column: x => x.SummonSanctionId,
                        principalSchema: "Dawem",
                        principalTable: "SummonSanctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SummonLogs_CompanyId",
                schema: "Dawem",
                table: "SummonLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonLogs_EmployeeId",
                schema: "Dawem",
                table: "SummonLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonLogs_SummonId",
                schema: "Dawem",
                table: "SummonLogs",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonLogSanctions_SummonLogId",
                schema: "Dawem",
                table: "SummonLogSanctions",
                column: "SummonLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonLogSanctions_SummonSanctionId",
                schema: "Dawem",
                table: "SummonLogSanctions",
                column: "SummonSanctionId");

            migrationBuilder.Sql("delete from dawem.Summons");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SummonLogSanctions",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "SummonLogs",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "EndDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.DropColumn(
                name: "LocalDateAndTime",
                schema: "Dawem",
                table: "Summons");

            migrationBuilder.RenameColumn(
                name: "StartDateAndTimeUTC",
                schema: "Dawem",
                table: "Summons",
                newName: "DateAndTime");

            migrationBuilder.CreateTable(
                name: "SummonMissingLogs",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    SummonId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Code = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SummonMissingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Dawem",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Dawem",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogs_Summons_SummonId",
                        column: x => x.SummonId,
                        principalSchema: "Dawem",
                        principalTable: "Summons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SummonMissingLogSanctions",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SummonMissingLogId = table.Column<int>(type: "int", nullable: false),
                    SummonSanctionId = table.Column<int>(type: "int", nullable: false),
                    AddUserId = table.Column<int>(type: "int", nullable: true),
                    AddedApplicationType = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisableReason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Done = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedApplicationType = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifyUserId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummonMissingLogSanctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogSanctions_SummonMissingLogs_SummonMissingLogId",
                        column: x => x.SummonMissingLogId,
                        principalSchema: "Dawem",
                        principalTable: "SummonMissingLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SummonMissingLogSanctions_SummonSanctions_SummonSanctionId",
                        column: x => x.SummonSanctionId,
                        principalSchema: "Dawem",
                        principalTable: "SummonSanctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogs_EmployeeId",
                schema: "Dawem",
                table: "SummonMissingLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogs_SummonId",
                schema: "Dawem",
                table: "SummonMissingLogs",
                column: "SummonId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "SummonMissingLogs",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogSanctions_SummonMissingLogId",
                schema: "Dawem",
                table: "SummonMissingLogSanctions",
                column: "SummonMissingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_SummonMissingLogSanctions_SummonSanctionId",
                schema: "Dawem",
                table: "SummonMissingLogSanctions",
                column: "SummonSanctionId");
        }
    }
}
