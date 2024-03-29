using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class changedConstraintsForItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Carts_CartId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Carts");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "Items",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Items",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Carts_CartId",
                table: "Items",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Carts_CartId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.AlterColumn<Guid>(
                name: "CartId",
                table: "Items",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Carts",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Carts_CartId",
                table: "Items",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
