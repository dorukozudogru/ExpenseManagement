using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class usedvehiclepurchaseadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsedVehiclePurchases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarModelId = table.Column<int>(nullable: false),
                    KM = table.Column<string>(nullable: false),
                    LicencePlate = table.Column<string>(nullable: false),
                    Seller = table.Column<string>(nullable: false),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    PurchasedSalesmanId = table.Column<int>(nullable: false),
                    PurchaseAmount = table.Column<double>(nullable: false),
                    PurchasedSalesmanBonus = table.Column<double>(nullable: false),
                    Buyer = table.Column<string>(nullable: false),
                    SaleDate = table.Column<DateTime>(nullable: true),
                    SoldSalesmansId = table.Column<int>(nullable: true),
                    SaleAmount = table.Column<double>(nullable: true),
                    SoldSalesmanBonus = table.Column<double>(nullable: true),
                    Profit = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedVehiclePurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedVehiclePurchases_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedVehiclePurchases_Salesmans_PurchasedSalesmanId",
                        column: x => x.PurchasedSalesmanId,
                        principalTable: "Salesmans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedVehiclePurchases_Salesmans_SoldSalesmansId",
                        column: x => x.SoldSalesmansId,
                        principalTable: "Salesmans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsedVehiclePurchases_CarModelId",
                table: "UsedVehiclePurchases",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedVehiclePurchases_PurchasedSalesmanId",
                table: "UsedVehiclePurchases",
                column: "PurchasedSalesmanId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedVehiclePurchases_SoldSalesmansId",
                table: "UsedVehiclePurchases",
                column: "SoldSalesmansId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsedVehiclePurchases");
        }
    }
}
