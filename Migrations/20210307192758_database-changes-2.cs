using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class databasechanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountCurrency",
                table: "EnergyMonthlies");

            migrationBuilder.RenameColumn(
                name: "Kw",
                table: "EnergyMonthlies",
                newName: "TAX");

            migrationBuilder.AddColumn<double>(
                name: "ConsumedKw",
                table: "EnergyMonthlies",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistributionFee",
                table: "EnergyMonthlies",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ProducedKw",
                table: "EnergyMonthlies",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumedKw",
                table: "EnergyMonthlies");

            migrationBuilder.DropColumn(
                name: "DistributionFee",
                table: "EnergyMonthlies");

            migrationBuilder.DropColumn(
                name: "ProducedKw",
                table: "EnergyMonthlies");

            migrationBuilder.RenameColumn(
                name: "TAX",
                table: "EnergyMonthlies",
                newName: "Kw");

            migrationBuilder.AddColumn<byte>(
                name: "AmountCurrency",
                table: "EnergyMonthlies",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
