using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CharitySale.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedItemValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Sales",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "SaleItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemId",
                table: "SaleItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "SaleItems",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Items",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.AddCheckConstraint(
                name: "CK_Sale_TotalAmount_NonNegative",
                table: "Sales",
                sql: "[TotalAmount] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SaleItem_Quantity_Positive",
                table: "SaleItems",
                sql: "[Quantity] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SaleItem_UnitPrice_NonNegative",
                table: "SaleItems",
                sql: "[UnitPrice] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Item_Price_Positive",
                table: "Items",
                sql: "[Price] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Item_Quantity_NonNegative",
                table: "Items",
                sql: "[Quantity] >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Sale_TotalAmount_NonNegative",
                table: "Sales");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SaleItem_Quantity_Positive",
                table: "SaleItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SaleItem_UnitPrice_NonNegative",
                table: "SaleItems");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Item_Price_Positive",
                table: "Items");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Item_Quantity_NonNegative",
                table: "Items");

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e10"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e11"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e12"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e13"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e14"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e15"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e16"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e17"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("81a130d2-502f-4cf1-a376-63edeb000e9f"));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sales",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "SaleId",
                table: "SaleItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "SaleItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SaleItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Category", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 0, "/images/brownie.jpg", "Brownie", 0.65m, 48 },
                    { 2, 0, "/images/muffin.jpg", "Muffin", 1.00m, 36 },
                    { 3, 0, "/images/cakepop.jpg", "Cake Pop", 1.35m, 24 },
                    { 4, 0, "/images/appletart.jpg", "Apple tart", 1.50m, 60 },
                    { 5, 0, "/images/water.jpg", "Water", 1.50m, 30 },
                    { 6, 1, "/images/shirt.jpg", "Shirt", 2.00m, 0 },
                    { 7, 1, "/images/pants.jpg", "Pants", 3.00m, 0 },
                    { 8, 1, "/images/jacket.jpg", "Jacket", 4.00m, 0 },
                    { 9, 1, "/images/toy.jpg", "Toy", 1.00m, 0 }
                });
        }
    }
}
