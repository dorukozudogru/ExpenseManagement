using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class raisediscounttrackadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaiseDiscountTracking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SectorId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FuelTypeId = table.Column<int>(nullable: false),
                    UnitSalePrice = table.Column<double>(nullable: false),
                    FuelRemainingAmount = table.Column<double>(nullable: false),
                    Difference = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaiseDiscountTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RaiseDiscountTracking_FuelTypes_FuelTypeId",
                        column: x => x.FuelTypeId,
                        principalTable: "FuelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaiseDiscountTracking_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaiseDiscountTracking_FuelTypeId",
                table: "RaiseDiscountTracking",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RaiseDiscountTracking_SectorId",
                table: "RaiseDiscountTracking",
                column: "SectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaiseDiscountTracking");
        }
    }
}
