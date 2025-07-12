using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketCatalogue.Commerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SettableAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Latitude",
                table: "Shops",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Longitude",
                table: "Shops",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Address_Latitude",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Address_Longitude",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "Shops");
        }
    }
}
