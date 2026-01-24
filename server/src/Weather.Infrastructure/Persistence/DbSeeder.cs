using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;

namespace Weather.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedCountriesAndCities(AppDbContext context)
    {
        // 1. Guard: Don't seed if data already exists
        if (await context.Countries.AnyAsync())
        {
            return;
        }

        Console.WriteLine("--- Starting Database Seeding ---");

        // 2. Seed Countries
        var countryMap = await SeedCountries(context);

        // 3. Seed Cities
        await SeedCities(context, countryMap);

        Console.WriteLine("--- Seeding Complete ---");
    }

    private static async Task<Dictionary<string, Country>> SeedCountries(AppDbContext context)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "data", "countries.json");
        if (!File.Exists(path)) throw new FileNotFoundException("Seed file missing: " + path);

        var countriesJson = await File.ReadAllTextAsync(path);
        using var doc = JsonDocument.Parse(countriesJson);

        var countryMap = new Dictionary<string, Country>();
        var countriesToInsert = new List<Country>();

        foreach (var element in doc.RootElement.EnumerateArray())
        {
            try
            {
                var cca2 = element.GetProperty("cca2").GetString()!;

                // Use your Entity Constructor
                var country = new Country(
                    name: element.GetProperty("name").GetProperty("common").GetString()!,
                    cca2: cca2,
                    cca3: element.GetProperty("cca3").GetString()!,
                    region: element.GetProperty("region").GetString()!,
                    subregion: element.TryGetProperty("subregion", out var sr) ? sr.GetString()! : "",
                    capital: element.TryGetProperty("capital", out var cap) && cap.GetArrayLength() > 0 ? cap[0].GetString()! : "",
                    latitude: element.GetProperty("latlng")[0].GetDouble(),
                    longitude: element.GetProperty("latlng")[1].GetDouble(),
                    independent: element.TryGetProperty("independent", out var ind) && ind.GetBoolean(),
                    landlocked: element.GetProperty("landlocked").GetBoolean(),
                    flag: element.GetProperty("flag").GetString()!
                );

                countriesToInsert.Add(country);
                countryMap[cca2] = country;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing country: {ex.Message}");
            }
        }

        await context.Countries.AddRangeAsync(countriesToInsert);
        await context.SaveChangesAsync();
        Console.WriteLine($"Inserted {countriesToInsert.Count} countries.");

        return countryMap;
    }

    private static async Task SeedCities(AppDbContext context, Dictionary<string, Country> countryMap)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "data", "cities15000.txt");
        if (!File.Exists(path)) return;

        var citiesToInsert = new List<City>();
        int count = 0;

        using var reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split('\t');
            var countryCode = parts[8];

            if (countryMap.TryGetValue(countryCode, out var country))
            {
                try
                {
                    var population = string.IsNullOrWhiteSpace(parts[14]) ? 0 : long.Parse(parts[14]);

                    var city = new City(
                        countryId: country.Id,
                        name: parts[1],
                        latitude: double.Parse(parts[4], CultureInfo.InvariantCulture),
                        longitude: double.Parse(parts[5], CultureInfo.InvariantCulture),
                        timezone: parts[17],
                        population
                    );

                    citiesToInsert.Add(city);
                    count++;

                    // Batch save every 2000 records to keep memory low
                    if (citiesToInsert.Count >= 2000)
                    {
                        await context.Cities.AddRangeAsync(citiesToInsert);
                        await context.SaveChangesAsync();
                        citiesToInsert.Clear();
                        Console.WriteLine($"Checkpointed {count} cities...");
                    }
                }
                catch (Exception ex)
                {
                    // Skip malformed lines
                    continue;
                }
            }
        }

        // Save remaining cities
        if (citiesToInsert.Any())
        {
            await context.Cities.AddRangeAsync(citiesToInsert);
            await context.SaveChangesAsync();
        }

        Console.WriteLine($"Inserted {count} cities total.");
    }

    public static async Task SeedWeatherStations(AppDbContext context)
    {
        Console.WriteLine("--- Starting Weather Station Sync ---");

        // 1. Find Cities that DO NOT have a station yet
        // This makes the seeder resumeable if it crashes mid-way
        var citiesWithoutStations = await context.Cities
            .AsNoTracking()
            .Where(city => !context.WeatherStations.Any(s => s.CityId == city.Id))
            .ToListAsync();

        if (!citiesWithoutStations.Any())
        {
            Console.WriteLine("All cities already have stations. Skipping.");
            return;
        }

        Console.WriteLine($"Found {citiesWithoutStations.Count} cities needing stations.");

        var stationsToInsert = new List<WeatherStation>();
        int count = 0;
        int totalToProcess = citiesWithoutStations.Count;

        foreach (var city in citiesWithoutStations)
        {
            var station = new WeatherStation(
                name: $"{city.Name} Station",
                latitude: city.Latitude,
                longitude: city.Longitude,
                cityId: city.Id
            );

            stationsToInsert.Add(station);
            count++;

            // Batch Save every 2000 to manage memory
            if (stationsToInsert.Count >= 100)
            {
                await context.WeatherStations.AddRangeAsync(stationsToInsert);
                await context.SaveChangesAsync();
                stationsToInsert.Clear();
                Console.WriteLine($"Progress: {count} / {totalToProcess} stations created...");
            }
        }

        // Save the final remainder
        if (stationsToInsert.Any())
        {
            await context.WeatherStations.AddRangeAsync(stationsToInsert);
            await context.SaveChangesAsync();
        }

        Console.WriteLine($"--- Success: Total of {count} new stations added ---");
    }
}