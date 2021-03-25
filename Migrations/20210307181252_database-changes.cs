using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class databasechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnergyYearlies");

            migrationBuilder.AlterColumn<double>(
                name: "Kw",
                table: "EnergyMonthlies",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<byte>(
                name: "AmountCurrency",
                table: "EnergyMonthlies",
                nullable: false,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Kw",
                table: "EnergyDailies",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Kw",
                table: "EnergyMonthlies",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<byte>(
                name: "AmountCurrency",
                table: "EnergyMonthlies",
                nullable: true,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<string>(
                name: "Kw",
                table: "EnergyDailies",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.CreateTable(
                name: "EnergyYearlies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConsumedKw = table.Column<string>(nullable: false),
                    DistributionFee = table.Column<string>(nullable: false),
                    EnergyMonthlyMonthId = table.Column<int>(nullable: false),
                    ProducedKw = table.Column<string>(nullable: false)
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
    }
}
