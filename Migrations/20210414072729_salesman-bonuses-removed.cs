using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class salesmanbonusesremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedSalesmanBonus",
                table: "UsedVehicleSales");

            migrationBuilder.DropColumn(
                name: "PurchasedSalesmanBonusCurrency",
                table: "UsedVehicleSales");

            migrationBuilder.DropColumn(
                name: "SoldSalesmanBonus",
                table: "UsedVehicleSales");

            migrationBuilder.DropColumn(
                name: "SoldSalesmanBonusCurrency",
                table: "UsedVehicleSales");

            migrationBuilder.DropColumn(
                name: "SalesmanBonus",
                table: "NewVehicleSales");

            migrationBuilder.DropColumn(
                name: "SalesmanBonusCurrency",
                table: "NewVehicleSales");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PurchasedSalesmanBonus",
                table: "UsedVehicleSales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<byte>(
                name: "PurchasedSalesmanBonusCurrency",
                table: "UsedVehicleSales",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<double>(
                name: "SoldSalesmanBonus",
                table: "UsedVehicleSales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<byte>(
                name: "SoldSalesmanBonusCurrency",
                table: "UsedVehicleSales",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<double>(
                name: "SalesmanBonus",
                table: "NewVehicleSales",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<byte>(
                name: "SalesmanBonusCurrency",
                table: "NewVehicleSales",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
