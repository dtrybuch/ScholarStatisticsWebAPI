using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class change_days : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfFridays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfMondays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfSaturdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfSundays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfThursdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfTuesdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountOfWednesdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfFridays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CountOfMondays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CountOfSaturdays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CountOfSundays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CountOfThursdays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CountOfTuesdays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CountOfWednesdays",
                table: "Categories");
        }
    }
}
