using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class change_to_days : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifferenceBetweenPublications",
                table: "Categories");

            migrationBuilder.AddColumn<double>(
                name: "DifferenceBetweenPublicationsInDays",
                table: "Categories",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifferenceBetweenPublicationsInDays",
                table: "Categories");

            migrationBuilder.AddColumn<DateTime>(
                name: "DifferenceBetweenPublications",
                table: "Categories",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
