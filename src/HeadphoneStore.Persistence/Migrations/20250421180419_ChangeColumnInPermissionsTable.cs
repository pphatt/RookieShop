using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadphoneStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnInPermissionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AppRoles_AppRoleId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_AppRoleId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "AppRoleId",
                table: "Permissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppRoleId",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_AppRoleId",
                table: "Permissions",
                column: "AppRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_AppRoles_AppRoleId",
                table: "Permissions",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id");
        }
    }
}
