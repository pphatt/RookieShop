using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadphoneStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImagesOrderOnProductMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ProductMedias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ProductMedias");
        }
    }
}
