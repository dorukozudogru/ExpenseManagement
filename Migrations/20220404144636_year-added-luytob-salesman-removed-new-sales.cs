using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class yearaddedluytobsalesmanremovednewsales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewVehicleSales_Salesmans_PurchasedSalesmanId",
                table: "NewVehicleSales");

            migrationBuilder.DropIndex(
                name: "IX_NewVehicleSales_PurchasedSalesmanId",
                table: "NewVehicleSales");

            migrationBuilder.DropColumn(
                name: "PurchasedSalesmanId",
                table: "NewVehicleSales");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "EnergyLuytobs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "EnergyLuytobs");

            migrationBuilder.AddColumn<int>(
                name: "PurchasedSalesmanId",
                table: "NewVehicleSales",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewVehicleSales_PurchasedSalesmanId",
                table: "NewVehicleSales",
                column: "PurchasedSalesmanId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewVehicleSales_Salesmans_PurchasedSalesmanId",
                table: "NewVehicleSales",
                column: "PurchasedSalesmanId",
                principalTable: "Salesmans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
