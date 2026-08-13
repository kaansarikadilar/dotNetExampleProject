using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_example.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePortfolioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Portfolio",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "UserBuyPrice",
                table: "Portfolio",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Portfolio");

            migrationBuilder.DropColumn(
                name: "UserBuyPrice",
                table: "Portfolio");
        }
    }
}
