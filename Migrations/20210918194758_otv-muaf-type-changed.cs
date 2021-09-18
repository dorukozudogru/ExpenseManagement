using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class otvmuaftypechanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "TaxExempt",
                table: "NewVehicleSales",
                nullable: false,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "TaxExempt",
                table: "NewVehicleSales",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
