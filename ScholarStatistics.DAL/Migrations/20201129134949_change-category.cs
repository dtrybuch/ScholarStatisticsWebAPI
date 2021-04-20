using Microsoft.EntityFrameworkCore.Migrations;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class changecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "CategoriesFK",
            table: "AffiliationCategories",
            newName: "CategoryFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
