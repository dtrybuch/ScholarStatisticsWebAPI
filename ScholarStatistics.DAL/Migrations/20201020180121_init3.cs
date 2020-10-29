using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryFK",
                table: "Publications");

            migrationBuilder.AddColumn<List<int>>(
                name: "CategoriesFK",
                table: "Publications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriesFK",
                table: "Publications");

            migrationBuilder.AddColumn<int>(
                name: "CategoryFK",
                table: "Publications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
