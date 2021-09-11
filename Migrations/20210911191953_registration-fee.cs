using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class registrationfee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationFee",
                table: "VehiclePurchases");

            migrationBuilder.AddColumn<int>(
                name: "RegistrationFeeId",
                table: "VehiclePurchases",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegistrationFees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegistrationFee = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationFees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePurchases_RegistrationFeeId",
                table: "VehiclePurchases",
                column: "RegistrationFeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehiclePurchases_RegistrationFees_RegistrationFeeId",
                table: "VehiclePurchases",
                column: "RegistrationFeeId",
                principalTable: "RegistrationFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehiclePurchases_RegistrationFees_RegistrationFeeId",
                table: "VehiclePurchases");

            migrationBuilder.DropTable(
                name: "RegistrationFees");

            migrationBuilder.DropIndex(
                name: "IX_VehiclePurchases_RegistrationFeeId",
                table: "VehiclePurchases");

            migrationBuilder.DropColumn(
                name: "RegistrationFeeId",
                table: "VehiclePurchases");

            migrationBuilder.AddColumn<double>(
                name: "RegistrationFee",
                table: "VehiclePurchases",
                nullable: true);
        }
    }
}
