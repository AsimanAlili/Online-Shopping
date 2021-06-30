using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Shopping.Data.Migrations
{
    public partial class IsBookmarkedAddedIntoProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBookmarked",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBookmarked",
                table: "Products");
        }
    }
}
