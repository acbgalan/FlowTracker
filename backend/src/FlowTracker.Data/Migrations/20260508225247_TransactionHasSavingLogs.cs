using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionHasSavingLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "SavingLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SavingLogs_TransactionId",
                table: "SavingLogs",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavingLogs_Transactions_TransactionId",
                table: "SavingLogs",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavingLogs_Transactions_TransactionId",
                table: "SavingLogs");

            migrationBuilder.DropIndex(
                name: "IX_SavingLogs_TransactionId",
                table: "SavingLogs");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "SavingLogs");
        }
    }
}
