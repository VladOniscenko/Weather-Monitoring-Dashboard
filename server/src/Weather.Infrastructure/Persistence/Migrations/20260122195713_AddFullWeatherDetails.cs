using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFullWeatherDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MaxTemp",
                table: "WeatherReadings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinTemp",
                table: "WeatherReadings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId1",
                table: "Cities",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId1",
                table: "Cities",
                column: "CountryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryId1",
                table: "Cities",
                column: "CountryId1",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Countries_CountryId1",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_CountryId1",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "MaxTemp",
                table: "WeatherReadings");

            migrationBuilder.DropColumn(
                name: "MinTemp",
                table: "WeatherReadings");

            migrationBuilder.DropColumn(
                name: "CountryId1",
                table: "Cities");
        }
    }
}
