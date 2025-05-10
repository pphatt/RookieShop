using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadphoneStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CorrectAggregateRoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "UserAddresses",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Transactions",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Products",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "ProductRatings",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "ProductMedias",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Permissions",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Orders",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "OrderPayments",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "OrderDetails",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Categories",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "Brands",
                newName: "ModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "AppUsers",
                newName: "ModifiedDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "UserAddresses",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "Transactions",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "Products",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "ProductRatings",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "ProductMedias",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "Permissions",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "Orders",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "OrderPayments",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "OrderDetails",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "Categories",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "Brands",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "ModifiedDateTime",
                table: "AppUsers",
                newName: "UpdatedDateTime");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
