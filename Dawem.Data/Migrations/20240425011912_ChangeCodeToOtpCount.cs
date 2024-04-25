using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dawem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCodeToOtpCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "EmployeeOTPs");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "Dawem",
                table: "EmployeeOTPs",
                newName: "OTPCount");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOTPs_CompanyId",
                schema: "Dawem",
                table: "EmployeeOTPs",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeOTPs_CompanyId",
                schema: "Dawem",
                table: "EmployeeOTPs");

            migrationBuilder.RenameColumn(
                name: "OTPCount",
                schema: "Dawem",
                table: "EmployeeOTPs",
                newName: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_CompanyId_Code_IsDeleted",
                schema: "Dawem",
                table: "EmployeeOTPs",
                columns: new[] { "CompanyId", "Code", "IsDeleted" },
                unique: true);
        }
    }
}
