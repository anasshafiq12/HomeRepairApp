using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRepairApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class correctspel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quanity",
                table: "SelectedCartItems",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "SelectedCartItems",
                newName: "Quanity");
        }
    }
}
