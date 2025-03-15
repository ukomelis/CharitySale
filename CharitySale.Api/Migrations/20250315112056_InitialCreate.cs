using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CharitySale.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.CheckConstraint("CK_Item_Price_Positive", "\"Price\" >= 0");
                    table.CheckConstraint("CK_Item_Quantity_NonNegative", "\"Quantity\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SaleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.CheckConstraint("CK_Sale_TotalAmount_NonNegative", "\"TotalAmount\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.CheckConstraint("CK_SaleItem_Quantity_Positive", "\"Quantity\" > 0");
                    table.CheckConstraint("CK_SaleItem_UnitPrice_NonNegative", "\"UnitPrice\" >= 0");
                    table.ForeignKey(
                        name: "FK_SaleItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Category", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e10"), 0, "/images/muffin.jpg", "Muffin", 1.00m, 36 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e11"), 0, "/images/cakepop.jpg", "Cake Pop", 1.35m, 24 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e12"), 0, "/images/appletart.jpg", "Apple tart", 1.50m, 60 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e13"), 0, "/images/water.jpg", "Water", 1.50m, 30 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e14"), 1, "/images/shirt.jpg", "Shirt", 2.00m, 0 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e15"), 1, "/images/pants.jpg", "Pants", 3.00m, 0 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e16"), 1, "/images/jacket.jpg", "Jacket", 4.00m, 0 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e17"), 1, "/images/toy.jpg", "Toy", 1.00m, 0 },
                    { new Guid("81a130d2-502f-4cf1-a376-63edeb000e9f"), 0, "/images/brownie.jpg", "Brownie", 0.65m, 48 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ItemId",
                table: "SaleItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItems",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
