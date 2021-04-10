using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class plateandpurchasedsalesmanadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicencePlate",
                table: "NewVehicleSales",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewVehicleSales_Salesmans_PurchasedSalesmanId",
                table: "NewVehicleSales");

            migrationBuilder.DropIndex(
                name: "IX_NewVehicleSales_PurchasedSalesmanId",
                table: "NewVehicleSales");

            migrationBuilder.DropColumn(
                name: "LicencePlate",
                table: "NewVehicleSales");

            migrationBuilder.DropColumn(
                name: "PurchasedSalesmanId",
                table: "NewVehicleSales");
        }
    }
}
