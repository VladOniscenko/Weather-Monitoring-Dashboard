using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "WeatherStations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WeatherStations_UserId",
                table: "WeatherStations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherStations_Users_UserId",
                table: "WeatherStations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherStations_Users_UserId",
                table: "WeatherStations");

            migrationBuilder.DropIndex(
                name: "IX_WeatherStations_UserId",
                table: "WeatherStations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WeatherStations");
        }
    }
}
