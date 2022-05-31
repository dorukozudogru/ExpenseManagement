using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class fieldsaddedtomyvehicles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentValue",
                table: "MyVehicles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "LicencePlate",
                table: "MyVehicles",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "MyVehicles");

            migrationBuilder.DropColumn(
                name: "LicencePlate",
                table: "MyVehicles");
        }
    }
}
