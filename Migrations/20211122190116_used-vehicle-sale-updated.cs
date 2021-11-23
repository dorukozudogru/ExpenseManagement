using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class usedvehiclesaleupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedVehicleSales_VehiclePurchases_VehiclePurchaseId",
                table: "UsedVehicleSales");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedVehicleSales_UsedVehiclePurchases_VehiclePurchaseId",
                table: "UsedVehicleSales",
                column: "VehiclePurchaseId",
                principalTable: "UsedVehiclePurchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsedVehicleSales_UsedVehiclePurchases_VehiclePurchaseId",
                table: "UsedVehicleSales");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedVehicleSales_VehiclePurchases_VehiclePurchaseId",
                table: "UsedVehicleSales",
                column: "VehiclePurchaseId",
                principalTable: "VehiclePurchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
