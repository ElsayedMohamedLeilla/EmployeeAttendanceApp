using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FingerprintDeviceLogFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeviceSerialNumber",
                schema: "Dawem",
                table: "FingerprintDeviceLogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DeviceSerialNumber",
                schema: "Dawem",
                table: "FingerprintDeviceLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
