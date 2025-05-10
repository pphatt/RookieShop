using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadphoneStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CleanUpProductAggregateRoot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductMedias");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProductMedias");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "ProductMedias",
                newName: "DisplayOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayOrder",
                table: "ProductMedias",
                newName: "Order");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ProductMedias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "ProductMedias",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
