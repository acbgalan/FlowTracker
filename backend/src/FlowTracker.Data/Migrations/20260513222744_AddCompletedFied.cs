using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedFied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "SavingGoals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Budgets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "SavingGoals");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Budgets");
        }
    }
}
