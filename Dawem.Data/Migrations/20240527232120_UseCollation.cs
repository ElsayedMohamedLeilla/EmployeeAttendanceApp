using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UseCollation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScreenId",
                schema: "Dawem",
                table: "PermissionScreens",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdentityCode",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CS_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionScreens_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens",
                column: "ScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionScreens_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens",
                column: "ScreenId",
                principalSchema: "Dawem",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionScreens_Screens_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.DropIndex(
                name: "IX_PermissionScreens_ScreenId",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.DropColumn(
                name: "ScreenId",
                schema: "Dawem",
                table: "PermissionScreens");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityCode",
                schema: "Dawem",
                table: "Companies",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");
        }
    }
}
