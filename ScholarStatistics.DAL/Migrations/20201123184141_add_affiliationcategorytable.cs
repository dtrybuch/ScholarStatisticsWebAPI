using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ScholarStatistics.DAL.Migrations
{
    public partial class add_affiliationcategorytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AffiliationCategories",
                columns: table => new
                {
                    AffiliationCategoryId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AffiliationFK = table.Column<int>(nullable: false),
                    CategoriesFK = table.Column<int>(nullable: false),
                    CountOfCategoryPublications = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliationCategories", x => x.AffiliationCategoryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffiliationCategories");
        }
    }
}
