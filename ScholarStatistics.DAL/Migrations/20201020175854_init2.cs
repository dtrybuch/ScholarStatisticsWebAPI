using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorFK",
                table: "Publications");

            migrationBuilder.AddColumn<List<int>>(
                name: "AuthorsFK",
                table: "Publications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorsFK",
                table: "Publications");

            migrationBuilder.AddColumn<int>(
                name: "AuthorFK",
                table: "Publications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
