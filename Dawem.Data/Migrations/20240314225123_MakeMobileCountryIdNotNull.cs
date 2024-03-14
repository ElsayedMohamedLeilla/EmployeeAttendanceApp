using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeMobileCountryIdNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowChangeFingerprintMobileCodeForOneTime",
                schema: "Dawem",
                table: "Employees",
                newName: "AllowChangeFingerprintMobileCode");


            migrationBuilder.Sql("update Dawem.MyUsers set MobileCountryId = 65");
            migrationBuilder.AlterColumn<int>(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.Sql("update Dawem.Employees set MobileCountryId = 65");
            migrationBuilder.AlterColumn<int>(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowChangeFingerprintMobileCode",
                schema: "Dawem",
                table: "Employees",
                newName: "AllowChangeFingerprintMobileCodeForOneTime");

            migrationBuilder.AlterColumn<int>(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "MyUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MobileCountryId",
                schema: "Dawem",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
