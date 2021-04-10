using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class taxratesaddedtopurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KDV",
                table: "VehiclePurchases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTV",
                table: "VehiclePurchases",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RegistrationFee",
                table: "VehiclePurchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KDV",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "OTV",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "RegistrationFee",
                table: "VehiclePurchases");
        }
    }
}
