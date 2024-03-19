using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDeviceCodeToMNobileCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FingerprintDeviceCode",
                schema: "Dawem",
                table: "Employees",
                newName: "FingerprintMobileCode");

            migrationBuilder.RenameColumn(
                name: "AllowChangeFingerprintDeviceCodeForOneTime",
                schema: "Dawem",
                table: "Employees",
                newName: "AllowChangeFingerprintMobileCodeForOneTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FingerprintMobileCode",
                schema: "Dawem",
                table: "Employees",
                newName: "FingerprintDeviceCode");

            migrationBuilder.RenameColumn(
                name: "AllowChangeFingerprintMobileCodeForOneTime",
                schema: "Dawem",
                table: "Employees",
                newName: "AllowChangeFingerprintDeviceCodeForOneTime");
        }
    }
}
