using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class petroladded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointOfSale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<byte>(nullable: false),
                    SectorId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    BankBranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOfSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointOfSale_BankBranches_BankBranchId",
                        column: x => x.BankBranchId,
                        principalTable: "BankBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PointOfSale_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointOfSale_BankBranchId",
                table: "PointOfSale",
                column: "BankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PointOfSale_SectorId",
                table: "PointOfSale",
                column: "SectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointOfSale");
        }
    }
}
