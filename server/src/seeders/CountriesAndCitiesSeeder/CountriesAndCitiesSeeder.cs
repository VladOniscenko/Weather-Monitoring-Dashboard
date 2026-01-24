using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Seeder.Seeders;

public static class CountriesAndCitiesSeerder
{
    private static readonly string BaseUrl = "http://localhost:5001/api";
    private static readonly string CountriesFile = "data/countries.json";
    private static readonly string CitiesFile = "data/cities15000.txt";

    public static async Task Seed()
    {
        int insertedCities = 0;
        int insertedCountries = 0;

        // 1. Login
        var token = await LoginAsync("admin", "admin");
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("Login failed");
            return;
        }
        Console.WriteLine("Logged in successfully.");

        // 2. Read and parse countries.json
        var countriesJson = await File.ReadAllTextAsync(CountriesFile);
        var countryDocs = JsonDocument.Parse(countriesJson).RootElement.EnumerateArray();

        var countryIds = new Dictionary<string, Guid>(); // map CCA2 -> ID

        foreach (var countryEl in countryDocs)
        {
            string cca2 = countryEl.GetProperty("cca2").GetString() ?? "";
            string cca3 = countryEl.GetProperty("cca3").GetString() ?? "";
            string region = countryEl.GetProperty("region").GetString() ?? "";
            string subregion = countryEl.GetProperty("subregion").GetString() ?? "";
            string flag = countryEl.GetProperty("flag").GetString() ?? "";

            // Null-safe booleans
            bool independent = countryEl.TryGetProperty("independent", out var indEl) && indEl.ValueKind == JsonValueKind.True;
            bool landlocked = countryEl.TryGetProperty("landlocked", out var landEl) && landEl.ValueKind == JsonValueKind.True;

            // Name extraction
            var nameCommon = countryEl.GetProperty("name").GetProperty("common").GetString() ?? "";

            // Capital may be empty array
            string capital = "";
            if (countryEl.TryGetProperty("capital", out var capitalEl) && capitalEl.ValueKind == JsonValueKind.Array && capitalEl.GetArrayLength() > 0)
                capital = capitalEl[0].GetString() ?? "";

            // Lat/Lng
            double lat = 0, lng = 0;
            if (countryEl.TryGetProperty("latlng", out var latlngEl) && latlngEl.ValueKind == JsonValueKind.Array && latlngEl.GetArrayLength() >= 2)
            {
                lat = latlngEl[0].GetDouble();
                lng = latlngEl[1].GetDouble();
            }

            var countryObj = new
            {
                Name = nameCommon,
                CCA2 = cca2,
                CCA3 = cca3,
                Region = region,
                Subregion = subregion,
                Capital = capital,
                Latitude = lat,
                Longitude = lng,
                Independent = independent,
                Landlocked = landlocked,
                Flag = flag
            };

            var id = await PostAndGetIdAsync("Countries", countryObj, token);
            if (id != Guid.Empty)
            {
                countryIds[cca2] = id;
                Console.WriteLine($"Inserted country: {nameCommon} -> {id}");
                insertedCountries++;
            }
        }

        // 3. Read cities.txt
        using var reader = new StreamReader(CitiesFile);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split('\t');
            try
            {
                string countryCode = parts[8]; // CCA2
                if (!countryIds.ContainsKey(countryCode))
                {
                    Console.WriteLine($"Skipping city {parts[1]}, country not found: {countryCode}");
                    continue;
                }

                var cityObj = new
                {
                    Name = parts[1],
                    Latitude = double.Parse(parts[4], CultureInfo.InvariantCulture),
                    Longitude = double.Parse(parts[5], CultureInfo.InvariantCulture),
                    Population = string.IsNullOrWhiteSpace(parts[14]) ? 0 : long.Parse(parts[14]),
                    Timezone = parts[17],
                    CountryId = countryIds[countryCode]
                };

                await PostAsync("Cities", cityObj, token);
                Console.WriteLine($"Inserted city: {cityObj.Name}");
                insertedCities++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing city line: {line} - {ex.Message}");
            }
        }

        Console.WriteLine("Seeding complete.");
        Console.WriteLine($"Inseted {insertedCities} Cities.");
        Console.WriteLine($"Insered {insertedCountries} Countries.");
    }

    private static async Task<string> LoginAsync(string email, string password)
    {
        using var client = new HttpClient();
        var loginData = new { Email = email, Password = password };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{BaseUrl}/Users/login", content);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var loginResponse = JsonSerializer.Deserialize<UserLoginResponseDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return loginResponse?.Token;
    }

    private static async Task PostAsync(string endpoint, object data, string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{BaseUrl}/{endpoint}", content);

        if (!response.IsSuccessStatusCode)
            Console.WriteLine($"Error posting to {endpoint}: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
    }

    private static async Task<Guid> PostAndGetIdAsync(string endpoint, object data, string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"{BaseUrl}/{endpoint}", content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error posting to {endpoint}: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            return Guid.Empty;
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("id", out var idProp) && Guid.TryParse(idProp.GetString(), out var id))
            return id;

        return Guid.Empty;
    }

    public record UserLoginResponseDto(string Token, DateTime ExpiresAt);
}