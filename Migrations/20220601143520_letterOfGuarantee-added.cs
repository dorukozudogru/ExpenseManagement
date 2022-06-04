using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class letterOfGuaranteeadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LetterOfGuaranteeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LetterOfGuaranteeId = table.Column<int>(nullable: false),
                    LetterOfGuaranteeImage = table.Column<byte[]>(nullable: true),
                    LetterOfGuaranteeImageFormat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterOfGuaranteeDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LetterOfGuarantees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Institution = table.Column<string>(nullable: false),
                    BankBranchId = table.Column<int>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    LetterOfGuaranteeCostId = table.Column<int>(nullable: false),
                    FinishDate = table.Column<DateTime>(nullable: false),
                    LetterOfGuaranteeImage = table.Column<byte[]>(nullable: true),
                    LetterOfGuaranteeImageFormat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterOfGuarantees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterOfGuarantees_BankBranches_BankBranchId",
                        column: x => x.BankBranchId,
                        principalTable: "BankBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LetterOfGuarantees_LetterOfGuaranteeCosts_LetterOfGuaranteeCostId",
                        column: x => x.LetterOfGuaranteeCostId,
                        principalTable: "LetterOfGuaranteeCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfGuarantees_BankBranchId",
                table: "LetterOfGuarantees",
                column: "BankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterOfGuarantees_LetterOfGuaranteeCostId",
                table: "LetterOfGuarantees",
                column: "LetterOfGuaranteeCostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterOfGuaranteeDocuments");

            migrationBuilder.DropTable(
                name: "LetterOfGuarantees");
        }
    }
}
