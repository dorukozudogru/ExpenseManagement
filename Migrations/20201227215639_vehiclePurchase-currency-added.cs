using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class vehiclePurchasecurrencyadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PurchaseAmountCurrency",
                table: "VehiclePurchases",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "SaleAmountCurrency",
                table: "VehiclePurchases",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseAmountCurrency",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "SaleAmountCurrency",
                table: "VehiclePurchases");
        }
    }
}
