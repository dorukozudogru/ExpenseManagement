using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class databasechanges3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invoice",
                table: "EnergyLuytobs");

            migrationBuilder.DropColumn(
                name: "InvoiceFormat",
                table: "EnergyLuytobs");

            migrationBuilder.DropColumn(
                name: "Luytob",
                table: "EnergyLuytobs");

            migrationBuilder.DropColumn(
                name: "LuytobFormat",
                table: "EnergyLuytobs");

            migrationBuilder.AddColumn<int>(
                name: "EnergyLuytobFileId",
                table: "EnergyLuytobs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EnergyLuytobFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Luytob = table.Column<byte[]>(nullable: true),
                    LuytobFormat = table.Column<string>(nullable: true),
                    Invoice = table.Column<byte[]>(nullable: true),
                    InvoiceFormat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyLuytobFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnergyLuytobs_EnergyLuytobFileId",
                table: "EnergyLuytobs",
                column: "EnergyLuytobFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnergyLuytobs_EnergyLuytobFiles_EnergyLuytobFileId",
                table: "EnergyLuytobs",
                column: "EnergyLuytobFileId",
                principalTable: "EnergyLuytobFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnergyLuytobs_EnergyLuytobFiles_EnergyLuytobFileId",
                table: "EnergyLuytobs");

            migrationBuilder.DropTable(
                name: "EnergyLuytobFiles");

            migrationBuilder.DropIndex(
                name: "IX_EnergyLuytobs_EnergyLuytobFileId",
                table: "EnergyLuytobs");

            migrationBuilder.DropColumn(
                name: "EnergyLuytobFileId",
                table: "EnergyLuytobs");

            migrationBuilder.AddColumn<byte[]>(
                name: "Invoice",
                table: "EnergyLuytobs",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<string>(
                name: "InvoiceFormat",
                table: "EnergyLuytobs",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Luytob",
                table: "EnergyLuytobs",
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<string>(
                name: "LuytobFormat",
                table: "EnergyLuytobs",
                nullable: true);
        }
    }
}
