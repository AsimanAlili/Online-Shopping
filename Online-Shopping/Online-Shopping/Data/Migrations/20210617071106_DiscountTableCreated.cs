using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Shopping.Data.Migrations
{
    public partial class DiscountTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    SubTitle = table.Column<string>(maxLength: 150, nullable: true),
                    SaleTitle = table.Column<string>(maxLength: 150, nullable: true),
                    Photo = table.Column<string>(maxLength: 100, nullable: true),
                    RedirectUrl = table.Column<string>(maxLength: 100, nullable: true),
                    DiscountTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");
        }
    }
}
