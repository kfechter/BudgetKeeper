using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BudgetKeeper.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class SubDebt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubDebt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DebtName = table.Column<string>(type: "text", nullable: true),
                    DebtAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    MonthlyPayment = table.Column<decimal>(type: "numeric", nullable: true),
                    BudgetItemId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubDebt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubDebt_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubDebt_BudgetItemId",
                table: "SubDebt",
                column: "BudgetItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubDebt");
        }
    }
}
