using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitMigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CCA2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CCA3 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Subregion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Capital = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Flag = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Independent = table.Column<bool>(type: "boolean", nullable: false),
                    Landlocked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Timezone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    Longitude = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherStations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeatherReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MainCondition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Temperature = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    FeelsLike = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    Min = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    Max = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    Pressure = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Humidity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SeaLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    GroundLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Visibility = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    WindSpeed = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: false),
                    WindDeg = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Cloudiness = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Rain = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: true, defaultValue: 0.0),
                    Snow = table.Column<double>(type: "double precision", precision: 9, scale: 6, nullable: true, defaultValue: 0.0),
                    CapturedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherReadings_WeatherStations_StationId",
                        column: x => x.StationId,
                        principalTable: "WeatherStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CCA2",
                table: "Countries",
                column: "CCA2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CCA3",
                table: "Countries",
                column: "CCA3",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherReadings_StationId",
                table: "WeatherReadings",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherStations_CountryId",
                table: "WeatherStations",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeatherReadings");

            migrationBuilder.DropTable(
                name: "WeatherStations");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
