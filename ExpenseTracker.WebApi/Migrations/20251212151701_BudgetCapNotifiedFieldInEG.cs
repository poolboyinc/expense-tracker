using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class BudgetCapNotifiedFieldInEG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BudgetCapNotified",
                table: "ExpenseGroup",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetCapNotified",
                table: "ExpenseGroup");
        }
    }
}
