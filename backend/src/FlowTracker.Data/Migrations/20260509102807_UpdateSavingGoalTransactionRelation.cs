using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSavingGoalTransactionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavingLogs");

            migrationBuilder.AddColumn<int>(
                name: "SavingGoalId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SavingGoalId",
                table: "Transactions",
                column: "SavingGoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SavingGoals_SavingGoalId",
                table: "Transactions",
                column: "SavingGoalId",
                principalTable: "SavingGoals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SavingGoals_SavingGoalId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SavingGoalId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SavingGoalId",
                table: "Transactions");

            migrationBuilder.CreateTable(
                name: "SavingLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavingGoalId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavingLogs_SavingGoals_SavingGoalId",
                        column: x => x.SavingGoalId,
                        principalTable: "SavingGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavingLogs_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavingLogs_SavingGoalId",
                table: "SavingLogs",
                column: "SavingGoalId");

            migrationBuilder.CreateIndex(
                name: "IX_SavingLogs_TransactionId",
                table: "SavingLogs",
                column: "TransactionId");
        }
    }
}
