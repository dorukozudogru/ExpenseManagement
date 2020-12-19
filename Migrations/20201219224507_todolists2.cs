using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseManagement.Migrations
{
    public partial class todolists2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "State",
                table: "ToDoLists",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "CollectAt",
                table: "ToDoLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectAt",
                table: "ToDoLists");

            migrationBuilder.AlterColumn<int>(
                name: "State",
                table: "ToDoLists",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
