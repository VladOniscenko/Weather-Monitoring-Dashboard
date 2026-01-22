using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Weather.Application.Common.Interfaces;

namespace Weather.Infrastructure.ExternalApi.OpenWeather;

public class OpenWeatherApi : IExternalWeatherApi
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenWeatherApi(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["WeatherApi:Key"] ?? throw new Exception("API Key missing");
    }

    public async Task<ExternalWeatherDto> GetWeatherForCoordinates(double lat, double lon)
    {
        var url = $"weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
        var response = await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(url);

        if (response == null || response.Weather.Count == 0) 
            throw new Exception("Incomplete data from OpenWeather.");

        var w = response.Weather[0];
        
        return new ExternalWeatherDto(
            MainCondition: w.Main,
            Description: w.Description,
            Icon: w.Icon,
            Temp: response.Main.Temp,
            FeelsLike: response.Main.Feels_Like,
            MinTemp: response.Main.Temp_Min,
            MaxTemp: response.Main.Temp_Max,
            Pressure: response.Main.Pressure,
            Humidity: response.Main.Humidity,
            SeaLevel: response.Main.Sea_Level,
            GroundLevel: response.Main.Grnd_Level,
            Visibility: response.Visibility,
            WindSpeed: response.Wind.Speed,
            WindDeg: response.Wind.Deg,
            Cloudiness: response.Clouds.All,
            Rain: response.Rain?.OneHour,
            Snow: response.Snow?.OneHour,
            CapturedAt: DateTimeOffset.FromUnixTimeSeconds(response.UnixTime).UtcDateTime
        );
    }
}