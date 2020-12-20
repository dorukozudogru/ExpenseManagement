using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class carbrandandmodel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Chassis",
                table: "VehiclePurchases",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PurchaseAmount",
                table: "VehiclePurchases",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "VehiclePurchases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "SaleAmount",
                table: "VehiclePurchases",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleDate",
                table: "VehiclePurchases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chassis",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "PurchaseAmount",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "SaleAmount",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "SaleDate",
                table: "VehiclePurchases");
        }
    }
}
