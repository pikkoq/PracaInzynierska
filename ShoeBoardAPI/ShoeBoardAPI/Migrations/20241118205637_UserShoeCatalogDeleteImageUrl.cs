using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserShoeCatalogDeleteImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_ShoeCatalogs_ShoeCatalogId",
                table: "Shoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_UserShoeCatalogs_UserShoeCatalogId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_UserShoeCatalogId",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "Image_Url",
                table: "UserShoeCatalogs");

            migrationBuilder.DropColumn(
                name: "ShoeAddType",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "UserShoeCatalogId",
                table: "Shoes");

            migrationBuilder.AlterColumn<string>(
                name: "Image_Path",
                table: "UserShoeCatalogs",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ShoeCatalogId",
                table: "Shoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_ShoeCatalogs_ShoeCatalogId",
                table: "Shoes",
                column: "ShoeCatalogId",
                principalTable: "ShoeCatalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_ShoeCatalogs_ShoeCatalogId",
                table: "Shoes");

            migrationBuilder.AlterColumn<string>(
                name: "Image_Path",
                table: "UserShoeCatalogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AddColumn<string>(
                name: "Image_Url",
                table: "UserShoeCatalogs",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ShoeCatalogId",
                table: "Shoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ShoeAddType",
                table: "Shoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserShoeCatalogId",
                table: "Shoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_UserShoeCatalogId",
                table: "Shoes",
                column: "UserShoeCatalogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_ShoeCatalogs_ShoeCatalogId",
                table: "Shoes",
                column: "ShoeCatalogId",
                principalTable: "ShoeCatalogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_UserShoeCatalogs_UserShoeCatalogId",
                table: "Shoes",
                column: "UserShoeCatalogId",
                principalTable: "UserShoeCatalogs",
                principalColumn: "Id");
        }
    }
}
