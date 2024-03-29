using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Migrations
{
    /// <inheritdoc />
    public partial class firstmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProductName = table.Column<string>(type: "longtext", nullable: false),
                    Price = table.Column<float>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogItems");
        }
    }
}
