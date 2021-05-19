using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class sectoraddedtofuelsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectorId",
                table: "FuelSales",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FuelSales_SectorId",
                table: "FuelSales",
                column: "SectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelSales_Sectors_SectorId",
                table: "FuelSales",
                column: "SectorId",
                principalTable: "Sectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelSales_Sectors_SectorId",
                table: "FuelSales");

            migrationBuilder.DropIndex(
                name: "IX_FuelSales_SectorId",
                table: "FuelSales");

            migrationBuilder.DropColumn(
                name: "SectorId",
                table: "FuelSales");
        }
    }
}
