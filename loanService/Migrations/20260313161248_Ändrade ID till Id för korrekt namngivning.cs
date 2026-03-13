using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loanService.Migrations
{
    /// <inheritdoc />
    public partial class ÄndradeIDtillIdförkorrektnamngivning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Loans",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ItemID",
                table: "Loans",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Loans",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Loans",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Loans",
                newName: "ItemID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Loans",
                newName: "ID");
        }
    }
}
