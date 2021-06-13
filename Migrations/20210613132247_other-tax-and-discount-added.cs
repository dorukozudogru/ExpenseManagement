using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class othertaxanddiscountadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DiscountCurrency",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OtherTAX",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OtherTAXCurrency",
                table: "Expenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "DiscountCurrency",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "OtherTAX",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "OtherTAXCurrency",
                table: "Expenses");
        }
    }
}
