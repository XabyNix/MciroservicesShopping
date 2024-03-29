using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class somechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Items",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Items",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Items");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId",
                table: "Items",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
