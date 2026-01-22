using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMinMaxFromWeather : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Max",
                table: "WeatherReadings");

            migrationBuilder.DropColumn(
                name: "Min",
                table: "WeatherReadings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Max",
                table: "WeatherReadings",
                type: "double precision",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Min",
                table: "WeatherReadings",
                type: "double precision",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
