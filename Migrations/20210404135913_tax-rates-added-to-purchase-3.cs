using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class taxratesaddedtopurchase3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RegistrationFee",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "IncludingRegistrationFee",
                table: "VehiclePurchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludingRegistrationFee",
                table: "VehiclePurchases");

            migrationBuilder.AlterColumn<double>(
                name: "RegistrationFee",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
