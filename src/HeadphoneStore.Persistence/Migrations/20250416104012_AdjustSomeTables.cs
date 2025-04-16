using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadphoneStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustSomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "UserAddresses",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "UserAddresses",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Transactions",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Transactions",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Products",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Products",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "ProductMedias",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "ProductMedias",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Permissions",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Permissions",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Orders",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Orders",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "OrderPayments",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "OrderPayments",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "OrderDetails",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "OrderDetails",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "Categories",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "Categories",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedOnUtc",
                table: "AppUsers",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                table: "AppUsers",
                newName: "CreatedDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "UserAddresses",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "UserAddresses",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Transactions",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "Transactions",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Products",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "Products",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "ProductMedias",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "ProductMedias",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Permissions",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "Permissions",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Orders",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "Orders",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "OrderPayments",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "OrderPayments",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "OrderDetails",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "OrderDetails",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Categories",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "Categories",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "AppUsers",
                newName: "ModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "AppUsers",
                newName: "CreatedOnUtc");
        }
    }
}
