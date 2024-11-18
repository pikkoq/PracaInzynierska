using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeBoardAPI.Migrations
{
    /// <inheritdoc />
    public partial class CasaceDropOnShoeDelite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Shoes_ShoeId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Shoes_ShoeId",
                table: "Posts",
                column: "ShoeId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Shoes_ShoeId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Shoes_ShoeId",
                table: "Posts",
                column: "ShoeId",
                principalTable: "Shoes",
                principalColumn: "Id");
        }
    }
}
