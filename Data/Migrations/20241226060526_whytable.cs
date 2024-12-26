using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRepairApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class whytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedCartItem_Carts_CartId",
                table: "SelectedCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectedCartItem",
                table: "SelectedCartItem");

            migrationBuilder.RenameTable(
                name: "SelectedCartItem",
                newName: "SelectedCartItems");

            migrationBuilder.RenameIndex(
                name: "IX_SelectedCartItem_CartId",
                table: "SelectedCartItems",
                newName: "IX_SelectedCartItems_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectedCartItems",
                table: "SelectedCartItems",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedCartItems_Carts_CartId",
                table: "SelectedCartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedCartItems_Carts_CartId",
                table: "SelectedCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectedCartItems",
                table: "SelectedCartItems");

            migrationBuilder.RenameTable(
                name: "SelectedCartItems",
                newName: "SelectedCartItem");

            migrationBuilder.RenameIndex(
                name: "IX_SelectedCartItems_CartId",
                table: "SelectedCartItem",
                newName: "IX_SelectedCartItem_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectedCartItem",
                table: "SelectedCartItem",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedCartItem_Carts_CartId",
                table: "SelectedCartItem",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
