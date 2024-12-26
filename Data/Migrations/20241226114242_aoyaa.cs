using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRepairApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class aoyaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SelectedCartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SelectedCartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
