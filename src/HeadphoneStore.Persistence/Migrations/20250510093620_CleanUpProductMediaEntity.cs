using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadphoneStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpProductMediaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedias_Products_ProductId",
                table: "ProductMedias");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedias_Products_ProductId",
                table: "ProductMedias",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedias_Products_ProductId",
                table: "ProductMedias");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedias_Products_ProductId",
                table: "ProductMedias",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
