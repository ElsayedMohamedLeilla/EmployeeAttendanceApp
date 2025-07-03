using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultType",
                schema: "Dawem",
                table: "DefaultLookups");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenDateUTC",
                schema: "Dawem",
                table: "FingerprintDevices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FingerprintDeviceUserCode",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShiftId",
                schema: "Dawem",
                table: "EmployeeAttendances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
