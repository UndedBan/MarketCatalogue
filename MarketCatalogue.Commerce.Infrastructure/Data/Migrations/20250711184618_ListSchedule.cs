using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketCatalogue.Commerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ListSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Schedule_CloseTime",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Schedule_Day",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Schedule_OpenTime",
                table: "Shops");

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => new { x.ShopId, x.Id });
                    table.ForeignKey(
                        name: "FK_Schedule_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Schedule_CloseTime",
                table: "Shops",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Schedule_Day",
                table: "Shops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Schedule_OpenTime",
                table: "Shops",
                type: "time",
                nullable: true);
        }
    }
}
