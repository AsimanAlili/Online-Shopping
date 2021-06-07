using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Shopping.Data.Migrations
{
    public partial class ProductColorAndProductSizeSomeChangedAndDescAndPhotoAddedIntoCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "ProductColors");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableSize",
                table: "ProductSizes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableColor",
                table: "ProductColors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Categories",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Categories",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableSize",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "IsAvailableColor",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Categories");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Sizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "ProductSizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "ProductColors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
