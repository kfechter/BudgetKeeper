using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetKeeper.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class BudgetTypeAndIsOpen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetType",
                table: "BudgetItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "BudgetItems",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetType",
                table: "BudgetItems");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "BudgetItems");
        }
    }
}
