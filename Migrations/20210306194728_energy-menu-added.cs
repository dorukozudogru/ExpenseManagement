using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class energymenuadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnergyDailies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Kw = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyDailies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyLuytobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Month = table.Column<byte>(nullable: false),
                    Luytob = table.Column<byte[]>(nullable: false),
                    LuytobFormat = table.Column<string>(nullable: true),
                    Invoice = table.Column<byte[]>(nullable: false),
                    InvoiceFormat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyLuytobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyMonthlies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Month = table.Column<byte>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    AmountCurrency = table.Column<byte>(nullable: true),
                    Kw = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyMonthlies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyYearlies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EnergyMonthlyMonthId = table.Column<int>(nullable: false),
                    ProducedKw = table.Column<string>(nullable: false),
                    ConsumedKw = table.Column<string>(nullable: false),
                    DistributionFee = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyYearlies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnergyYearlies_EnergyMonthlies_EnergyMonthlyMonthId",
                        column: x => x.EnergyMonthlyMonthId,
                        principalTable: "EnergyMonthlies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnergyYearlies_EnergyMonthlyMonthId",
                table: "EnergyYearlies",
                column: "EnergyMonthlyMonthId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyDailies");

            migrationBuilder.DropTable(
                name: "EnergyLuytobs");

            migrationBuilder.DropTable(
                name: "EnergyYearlies");

            migrationBuilder.DropTable(
                name: "EnergyMonthlies");
        }
    }
}
