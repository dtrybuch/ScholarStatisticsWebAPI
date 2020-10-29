using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class addcity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Affiliations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Affiliations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Affiliations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Affiliations");
        }
    }
}
