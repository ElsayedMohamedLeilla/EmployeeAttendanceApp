using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class DecimalAndFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.AlterColumn<decimal>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<decimal>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "decimal(30,20)",
                precision: 30,
                scale: 20,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "EmployeeAttendanceChecks",
                schema: "Dawem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeAttendanceId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(30,20)", precision: 30, scale: 20, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FingerPrintType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_EmployeeAttendanceChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAttendanceChecks_EmployeeAttendances_EmployeeAttendanceId",
                        column: x => x.EmployeeAttendanceId,
                        principalSchema: "Dawem",
                        principalTable: "EmployeeAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendanceChecks_EmployeeAttendanceId",
                schema: "Dawem",
                table: "EmployeeAttendanceChecks",
                column: "EmployeeAttendanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAttendanceChecks",
                schema: "Dawem");

            migrationBuilder.DropColumn(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "EmployeeAttendances");

            migrationBuilder.AlterColumn<double>(
                name: "AllowedMinutes",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,20)",
                oldPrecision: 30,
                oldScale: 20);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckInTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckOutTime",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "time",
                nullable: true);
        }
    }
}
