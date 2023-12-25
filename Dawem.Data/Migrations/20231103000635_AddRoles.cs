using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId1",
                schema: "Dawem",
                table: "UserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                schema: "Dawem",
                table: "UserRoles",
                type: "int",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "JustificationsTypes",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyId = table.Column<int>(type: "int", nullable: false),
            //        Code = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        AddUserId = table.Column<int>(type: "int", nullable: true),
            //        ModifyUserId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_JustificationsTypes", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_JustificationsTypes_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalSchema: "Dawem",
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PermissionsTypes",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyId = table.Column<int>(type: "int", nullable: false),
            //        Code = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        AddUserId = table.Column<int>(type: "int", nullable: true),
            //        ModifyUserId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PermissionsTypes", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_PermissionsTypes_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalSchema: "Dawem",
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "VacationsTypes",
            //    schema: "Dawem",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyId = table.Column<int>(type: "int", nullable: false),
            //        Code = table.Column<int>(type: "int", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        AddUserId = table.Column<int>(type: "int", nullable: true),
            //        ModifyUserId = table.Column<int>(type: "int", nullable: true),
            //        IsActive = table.Column<bool>(type: "bit", nullable: false),
            //        IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            //        DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_VacationsTypes", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_VacationsTypes_Companies_CompanyId",
            //            column: x => x.CompanyId,
            //            principalSchema: "Dawem",
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId1",
                schema: "Dawem",
                table: "UserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId1",
                schema: "Dawem",
                table: "UserRoles",
                column: "UserId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_JustificationsTypes_CompanyId",
            //    schema: "Dawem",
            //    table: "JustificationsTypes",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PermissionsTypes_CompanyId",
            //    schema: "Dawem",
            //    table: "PermissionsTypes",
            //    column: "CompanyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_VacationsTypes_CompanyId",
            //    schema: "Dawem",
            //    table: "VacationsTypes",
            //    column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_MyUsers_UserId1",
                schema: "Dawem",
                table: "UserRoles",
                column: "UserId1",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                schema: "Dawem",
                table: "UserRoles",
                column: "RoleId1",
                principalSchema: "Dawem",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_MyUsers_UserId1",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "JustificationsTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "PermissionsTypes",
                schema: "Dawem");

            migrationBuilder.DropTable(
                name: "VacationsTypes",
                schema: "Dawem");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RoleId1",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId1",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "Dawem",
                table: "UserRoles");
        }
    }
}
