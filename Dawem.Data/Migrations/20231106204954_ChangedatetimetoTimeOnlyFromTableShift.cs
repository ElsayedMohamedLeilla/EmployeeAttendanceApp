using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedatetimetoTimeOnlyFromTableShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLogs_MyUsers_UserId",
                schema: "Dawem",
                table: "ActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MyUsers_Branches_BranchId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MyUsers_Companies_CompanyId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_MyUsers_Employees_EmployeeId",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                schema: "Dawem",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_MyUsers_UserId",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_MyUsers_UserId",
                schema: "Dawem",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_MyUsers_UserId",
                schema: "Dawem",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogIns_MyUsers_UserId",
                schema: "Dawem",
                table: "UserLogIns");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_MyUsers_UserId",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserScreenActionPermissions_MyUsers_UserId",
                schema: "Dawem",
                table: "UserScreenActionPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_MyUsers_UserId",
                schema: "Dawem",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                schema: "Dawem",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Dawem",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogIns",
                schema: "Dawem",
                table: "UserLogIns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                schema: "Dawem",
                table: "UserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                schema: "Dawem",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleClaims",
                schema: "Dawem",
                table: "RoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyUsers",
                schema: "Dawem",
                table: "MyUsers");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "Dawem",
                newName: "AspNetUserTokens",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Dawem",
                newName: "AspNetUserRoles",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserLogIns",
                schema: "Dawem",
                newName: "AspNetUserLogins",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "Dawem",
                newName: "AspNetUserClaims",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Dawem",
                newName: "AspNetRoles",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "Dawem",
                newName: "AspNetRoleClaims",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "MyUsers",
                schema: "Dawem",
                newName: "AspNetUsers",
                newSchema: "Dawem");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Dawem",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogIns_UserId",
                schema: "Dawem",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaims_UserId",
                schema: "Dawem",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Dawem",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_MyUsers_EmployeeId",
                schema: "Dawem",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_MyUsers_CompanyId",
                schema: "Dawem",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_MyUsers_BranchId",
                schema: "Dawem",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_BranchId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckOutTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckInTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "RoleId1",
                schema: "Dawem",
                table: "AspNetUserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                schema: "Dawem",
                table: "AspNetUserRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "Dawem",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "Dawem",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                schema: "Dawem",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                schema: "Dawem",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "Dawem",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                schema: "Dawem",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "Dawem",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                schema: "Dawem",
                table: "AspNetUserRoles",
                column: "RoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId1",
                schema: "Dawem",
                table: "AspNetUserRoles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLogs_AspNetUsers_UserId",
                schema: "Dawem",
                table: "ActionLogs",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Dawem",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalSchema: "Dawem",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserClaims",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserLogins",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Dawem",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalSchema: "Dawem",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                schema: "Dawem",
                table: "AspNetUserRoles",
                column: "RoleId1",
                principalSchema: "Dawem",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserRoles",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                schema: "Dawem",
                table: "AspNetUserRoles",
                column: "UserId1",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Branches_BranchId",
                schema: "Dawem",
                table: "AspNetUsers",
                column: "BranchId",
                principalSchema: "Dawem",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                schema: "Dawem",
                table: "AspNetUsers",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                schema: "Dawem",
                table: "AspNetUsers",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserTokens",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_AspNetUsers_UserId",
                schema: "Dawem",
                table: "UserBranches",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_UserId",
                schema: "Dawem",
                table: "UserGroups",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserScreenActionPermissions_AspNetUsers_UserId",
                schema: "Dawem",
                table: "UserScreenActionPermissions",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionLogs_AspNetUsers_UserId",
                schema: "Dawem",
                table: "ActionLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                schema: "Dawem",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId1",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId1",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Branches_BranchId",
                schema: "Dawem",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_CompanyId",
                schema: "Dawem",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                schema: "Dawem",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                schema: "Dawem",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_AspNetUsers_UserId",
                schema: "Dawem",
                table: "UserBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_UserId",
                schema: "Dawem",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserScreenActionPermissions_AspNetUsers_UserId",
                schema: "Dawem",
                table: "UserScreenActionPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                schema: "Dawem",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "Dawem",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_RoleId1",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId1",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                schema: "Dawem",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                schema: "Dawem",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                schema: "Dawem",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                schema: "Dawem",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                schema: "Dawem",
                table: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "Dawem",
                newName: "UserTokens",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "Dawem",
                newName: "MyUsers",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "Dawem",
                newName: "UserRoles",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "Dawem",
                newName: "UserLogIns",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "Dawem",
                newName: "UserClaims",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "Dawem",
                newName: "Roles",
                newSchema: "Dawem");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "Dawem",
                newName: "RoleClaims",
                newSchema: "Dawem");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_EmployeeId",
                schema: "Dawem",
                table: "MyUsers",
                newName: "IX_MyUsers_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CompanyId",
                schema: "Dawem",
                table: "MyUsers",
                newName: "IX_MyUsers_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_BranchId",
                schema: "Dawem",
                table: "MyUsers",
                newName: "IX_MyUsers_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Dawem",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Dawem",
                table: "UserLogIns",
                newName: "IX_UserLogIns_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Dawem",
                table: "UserClaims",
                newName: "IX_UserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Dawem",
                table: "RoleClaims",
                newName: "IX_RoleClaims_RoleId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckOutTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckInTime",
                schema: "Dawem",
                table: "ShiftWorkingTimes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                schema: "Dawem",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyUsers",
                schema: "Dawem",
                table: "MyUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Dawem",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogIns",
                schema: "Dawem",
                table: "UserLogIns",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                schema: "Dawem",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                schema: "Dawem",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleClaims",
                schema: "Dawem",
                table: "RoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionLogs_MyUsers_UserId",
                schema: "Dawem",
                table: "ActionLogs",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyUsers_Branches_BranchId",
                schema: "Dawem",
                table: "MyUsers",
                column: "BranchId",
                principalSchema: "Dawem",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyUsers_Companies_CompanyId",
                schema: "Dawem",
                table: "MyUsers",
                column: "CompanyId",
                principalSchema: "Dawem",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyUsers_Employees_EmployeeId",
                schema: "Dawem",
                table: "MyUsers",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                schema: "Dawem",
                table: "RoleClaims",
                column: "RoleId",
                principalSchema: "Dawem",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_MyUsers_UserId",
                schema: "Dawem",
                table: "UserBranches",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_MyUsers_UserId",
                schema: "Dawem",
                table: "UserClaims",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_MyUsers_UserId",
                schema: "Dawem",
                table: "UserGroups",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogIns_MyUsers_UserId",
                schema: "Dawem",
                table: "UserLogIns",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_MyUsers_UserId",
                schema: "Dawem",
                table: "UserRoles",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "Dawem",
                table: "UserRoles",
                column: "RoleId",
                principalSchema: "Dawem",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserScreenActionPermissions_MyUsers_UserId",
                schema: "Dawem",
                table: "UserScreenActionPermissions",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_MyUsers_UserId",
                schema: "Dawem",
                table: "UserTokens",
                column: "UserId",
                principalSchema: "Dawem",
                principalTable: "MyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
