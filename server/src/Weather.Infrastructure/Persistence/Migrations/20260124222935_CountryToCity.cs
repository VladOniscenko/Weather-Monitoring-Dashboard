using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CountryToCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherStations_Countries_CountryId",
                table: "WeatherStations");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "WeatherStations",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_WeatherStations_CountryId",
                table: "WeatherStations",
                newName: "IX_WeatherStations_CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherStations_Cities_CityId",
                table: "WeatherStations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherStations_Cities_CityId",
                table: "WeatherStations");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "WeatherStations",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_WeatherStations_CityId",
                table: "WeatherStations",
                newName: "IX_WeatherStations_CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherStations_Countries_CountryId",
                table: "WeatherStations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
