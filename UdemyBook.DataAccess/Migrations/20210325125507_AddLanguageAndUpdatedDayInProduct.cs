using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UdemyBook.DataAccess.Migrations
{
    public partial class AddLanguageAndUpdatedDayInProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedDay",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "updatedDay",
                table: "Products");
        }
    }
}
