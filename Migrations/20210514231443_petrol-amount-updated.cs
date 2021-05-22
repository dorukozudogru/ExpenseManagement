using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class petrolamountupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "PointOfSale",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<byte>(
                name: "AmountCurrency",
                table: "PointOfSale",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PointOfSale");

            migrationBuilder.DropColumn(
                name: "AmountCurrency",
                table: "PointOfSale");
        }
    }
}
