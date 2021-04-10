using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class taxratesaddedtopurchase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OTV",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "KDV",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OTVPercent",
                table: "VehiclePurchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTVPercent",
                table: "VehiclePurchases");

            migrationBuilder.AlterColumn<int>(
                name: "OTV",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KDV",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
