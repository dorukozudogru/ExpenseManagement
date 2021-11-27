using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class netkarlilikadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetrolNetProfit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<byte>(nullable: false),
                    GrossProfit = table.Column<double>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    NetProfit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetrolNetProfit", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetrolNetProfit");
        }
    }
}
