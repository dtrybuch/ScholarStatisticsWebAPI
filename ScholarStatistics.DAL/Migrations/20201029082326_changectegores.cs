using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class changectegores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DifferenceBetweenPublications",
                table: "Categories",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<float>(
                name: "RatioPublications",
                table: "Categories",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DifferenceBetweenPublications",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RatioPublications",
                table: "Categories");
        }
    }
}
