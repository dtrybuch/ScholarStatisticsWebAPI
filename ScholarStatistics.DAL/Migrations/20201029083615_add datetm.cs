using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class adddatetm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DifferenceBetweenPublications",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DifferenceBetweenPublications",
                table: "Categories",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
