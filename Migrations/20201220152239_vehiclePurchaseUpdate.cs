using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class vehiclePurchaseUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SaleDate",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<double>(
                name: "SaleAmount",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(byte));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SaleDate",
                table: "VehiclePurchases",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "SaleAmount",
                table: "VehiclePurchases",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
