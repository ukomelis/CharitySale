using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharitySale.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAmountPaidAndChangeToSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Sales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ChangeAmount",
                table: "Sales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ChangeAmount",
                table: "Sales");
        }
    }
}
