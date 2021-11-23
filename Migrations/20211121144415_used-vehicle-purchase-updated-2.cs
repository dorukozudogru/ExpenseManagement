using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class usedvehiclepurchaseupdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Buyer",
                table: "UsedVehiclePurchases",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Buyer",
                table: "UsedVehiclePurchases",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
