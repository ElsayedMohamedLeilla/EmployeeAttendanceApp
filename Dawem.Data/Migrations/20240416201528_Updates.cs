using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                schema: "Dawem",
                table: "CompanyBranches");

            migrationBuilder.DropColumn(
                name: "HeadquarterLocation",
                schema: "Dawem",
                table: "Companies");

            //migrationBuilder.AlterColumn<int>(
            //    name: "EmployeeId",
            //    schema: "Dawem",
            //    table: "MyUsers",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Bool",
                schema: "Dawem",
                table: "DawemSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SettingType",
                schema: "Dawem",
                table: "DawemSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "Dawem",
                table: "CompanyBranches",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "Dawem",
                table: "CompanyBranches",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HeadquarterLocationLatitude",
                schema: "Dawem",
                table: "Companies",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HeadquarterLocationLongtude",
                schema: "Dawem",
                table: "Companies",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bool",
                schema: "Dawem",
                table: "DawemSettings");

            migrationBuilder.DropColumn(
                name: "SettingType",
                schema: "Dawem",
                table: "DawemSettings");

            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "Dawem",
                table: "CompanyBranches");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "Dawem",
                table: "CompanyBranches");

            migrationBuilder.DropColumn(
                name: "HeadquarterLocationLatitude",
                schema: "Dawem",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "HeadquarterLocationLongtude",
                schema: "Dawem",
                table: "Companies");

            //migrationBuilder.AlterColumn<int>(
            //    name: "EmployeeId",
            //    schema: "Dawem",
            //    table: "MyUsers",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "Dawem",
                table: "CompanyBranches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadquarterLocation",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
