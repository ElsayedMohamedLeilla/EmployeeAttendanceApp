using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStartEnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDay",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "EndMonth",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "EndYear",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "StartDay",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "StartYear",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "Dawem",
                table: "Holidays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "Dawem",
                table: "Holidays",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "Dawem",
                table: "Holidays");

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                schema: "Dawem",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndMonth",
                schema: "Dawem",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndYear",
                schema: "Dawem",
                table: "Holidays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartDay",
                schema: "Dawem",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                schema: "Dawem",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                schema: "Dawem",
                table: "Holidays",
                type: "int",
                nullable: true);
        }
    }
}
