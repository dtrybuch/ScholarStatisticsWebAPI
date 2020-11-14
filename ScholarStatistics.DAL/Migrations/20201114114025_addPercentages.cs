using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class addPercentages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PercentageOfFridays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfMondays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfSaturdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfSundays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfThursdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfTuesdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfWednesdays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageOfFridays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PercentageOfMondays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PercentageOfSaturdays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PercentageOfSundays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PercentageOfThursdays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PercentageOfTuesdays",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PercentageOfWednesdays",
                table: "Categories");
        }
    }
}
