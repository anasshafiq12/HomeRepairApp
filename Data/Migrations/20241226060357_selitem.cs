using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRepairApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class selitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectedCartItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    PriceWrtQuanity = table.Column<float>(type: "real", nullable: false),
                    SelectedQuantityByUser = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedCartItem", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_SelectedCartItem_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedCartItem_CartId",
                table: "SelectedCartItem",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedCartItem");
        }
    }
}
