using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class deleteAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorsFK",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "AffiliationFK",
                table: "Authors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "AuthorsFK",
                table: "Publications",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AffiliationFK",
                table: "Authors",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
