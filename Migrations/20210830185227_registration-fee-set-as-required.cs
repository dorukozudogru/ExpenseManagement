using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class registrationfeesetasrequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IncludingRegistrationFee",
                table: "VehiclePurchases",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IncludingRegistrationFee",
                table: "VehiclePurchases",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
