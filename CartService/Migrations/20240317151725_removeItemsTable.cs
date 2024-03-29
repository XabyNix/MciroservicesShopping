using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class removeItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CartId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Price = table.Column<float>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CartId",
                table: "Items",
                column: "CartId");
        }
    }
}
