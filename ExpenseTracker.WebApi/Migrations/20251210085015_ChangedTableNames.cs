using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseGroups_Users_UserId",
                table: "ExpenseGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseGroups_ExpenseGroupId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeGroups_Users_UserId",
                table: "IncomeGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_IncomeGroups_IncomeGroupId",
                table: "Incomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users_UserId",
                table: "Incomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomeGroups",
                table: "IncomeGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseGroups",
                table: "ExpenseGroups");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Incomes",
                newName: "Income");

            migrationBuilder.RenameTable(
                name: "IncomeGroups",
                newName: "IncomeGroup");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameTable(
                name: "ExpenseGroups",
                newName: "ExpenseGroup");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_UserId",
                table: "Income",
                newName: "IX_Income_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_IncomeGroupId",
                table: "Income",
                newName: "IX_Income_IncomeGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_IncomeGroups_UserId",
                table: "IncomeGroup",
                newName: "IX_IncomeGroup_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserId",
                table: "Expense",
                newName: "IX_Expense_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_ExpenseGroupId",
                table: "Expense",
                newName: "IX_Expense_ExpenseGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseGroups_UserId",
                table: "ExpenseGroup",
                newName: "IX_ExpenseGroup_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Income",
                table: "Income",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomeGroup",
                table: "IncomeGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseGroup",
                table: "ExpenseGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_ExpenseGroup_ExpenseGroupId",
                table: "Expense",
                column: "ExpenseGroupId",
                principalTable: "ExpenseGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_User_UserId",
                table: "Expense",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseGroup_User_UserId",
                table: "ExpenseGroup",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Income_IncomeGroup_IncomeGroupId",
                table: "Income",
                column: "IncomeGroupId",
                principalTable: "IncomeGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Income_User_UserId",
                table: "Income",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeGroup_User_UserId",
                table: "IncomeGroup",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_ExpenseGroup_ExpenseGroupId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_User_UserId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseGroup_User_UserId",
                table: "ExpenseGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_IncomeGroup_IncomeGroupId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_User_UserId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeGroup_User_UserId",
                table: "IncomeGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IncomeGroup",
                table: "IncomeGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Income",
                table: "Income");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseGroup",
                table: "ExpenseGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "IncomeGroup",
                newName: "IncomeGroups");

            migrationBuilder.RenameTable(
                name: "Income",
                newName: "Incomes");

            migrationBuilder.RenameTable(
                name: "ExpenseGroup",
                newName: "ExpenseGroups");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_IncomeGroup_UserId",
                table: "IncomeGroups",
                newName: "IX_IncomeGroups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Income_UserId",
                table: "Incomes",
                newName: "IX_Incomes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Income_IncomeGroupId",
                table: "Incomes",
                newName: "IX_Incomes_IncomeGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseGroup_UserId",
                table: "ExpenseGroups",
                newName: "IX_ExpenseGroups_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_UserId",
                table: "Expenses",
                newName: "IX_Expenses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_ExpenseGroupId",
                table: "Expenses",
                newName: "IX_Expenses_ExpenseGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IncomeGroups",
                table: "IncomeGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseGroups",
                table: "ExpenseGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseGroups_Users_UserId",
                table: "ExpenseGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseGroups_ExpenseGroupId",
                table: "Expenses",
                column: "ExpenseGroupId",
                principalTable: "ExpenseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_IncomeGroups_IncomeGroupId",
                table: "Incomes",
                column: "IncomeGroupId",
                principalTable: "IncomeGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users_UserId",
                table: "Incomes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
