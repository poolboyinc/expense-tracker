using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToAllGroupsAndSetCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "IncomeGroups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExpenseGroups",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeGroups_UserId",
                table: "IncomeGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseGroups_UserId",
                table: "ExpenseGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseGroups_Users_UserId",
                table: "ExpenseGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeGroups_Users_UserId",
                table: "IncomeGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseGroups_Users_UserId",
                table: "ExpenseGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeGroups_Users_UserId",
                table: "IncomeGroups");

            migrationBuilder.DropIndex(
                name: "IX_IncomeGroups_UserId",
                table: "IncomeGroups");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseGroups_UserId",
                table: "ExpenseGroups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IncomeGroups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExpenseGroups");
        }
    }
}
