using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class somechanges3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Carts_CartId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CartId",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Items_CartId",
                table: "Items",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Carts_CartId",
                table: "Items",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
