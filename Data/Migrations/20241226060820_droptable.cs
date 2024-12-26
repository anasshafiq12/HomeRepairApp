using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRepairApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class droptable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "Quanity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quanity",
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
    }
}
