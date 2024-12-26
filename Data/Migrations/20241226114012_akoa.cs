using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRepairApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class akoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SelectedCartItems",
                newName: "SelectedQuantityByUser");

            migrationBuilder.AddColumn<int>(
                name: "AvailableQuantity",
                table: "SelectedCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "PriceWrtQuanity",
                table: "SelectedCartItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SelectedCartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableQuantity",
                table: "SelectedCartItems");

            migrationBuilder.DropColumn(
                name: "PriceWrtQuanity",
                table: "SelectedCartItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SelectedCartItems");

            migrationBuilder.RenameColumn(
                name: "SelectedQuantityByUser",
                table: "SelectedCartItems",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "CartId");
        }
    }
}
