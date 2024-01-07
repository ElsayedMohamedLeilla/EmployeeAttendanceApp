using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiyNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullMessege",
                schema: "Dawem",
                table: "NotificationStores");

            migrationBuilder.DropColumn(
                name: "ShortMessege",
                schema: "Dawem",
                table: "NotificationStores");

            migrationBuilder.RenameColumn(
                name: "RecipientUserId",
                schema: "Dawem",
                table: "NotificationStores",
                newName: "NotificationType");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                schema: "Dawem",
                table: "NotificationStores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationStores_EmployeeId",
                schema: "Dawem",
                table: "NotificationStores",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationStores_Employees_EmployeeId",
                schema: "Dawem",
                table: "NotificationStores",
                column: "EmployeeId",
                principalSchema: "Dawem",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationStores_Employees_EmployeeId",
                schema: "Dawem",
                table: "NotificationStores");

            migrationBuilder.DropIndex(
                name: "IX_NotificationStores_EmployeeId",
                schema: "Dawem",
                table: "NotificationStores");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Dawem",
                table: "NotificationStores");

            migrationBuilder.RenameColumn(
                name: "NotificationType",
                schema: "Dawem",
                table: "NotificationStores",
                newName: "RecipientUserId");

            migrationBuilder.AddColumn<string>(
                name: "FullMessege",
                schema: "Dawem",
                table: "NotificationStores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortMessege",
                schema: "Dawem",
                table: "NotificationStores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
