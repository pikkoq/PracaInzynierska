using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class ShoeCatalogIdGeneratedShopUrlAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopUrl",
                table: "UserShoeCatalogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopUrl",
                table: "UserShoeCatalogs");
        }
    }
}
