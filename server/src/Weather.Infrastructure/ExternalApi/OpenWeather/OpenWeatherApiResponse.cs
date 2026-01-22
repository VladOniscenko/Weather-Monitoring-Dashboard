using System.Text.Json.Serialization;

namespace Weather.Infrastructure.ExternalApi.OpenWeather;

public record OpenWeatherResponse(
    [property: JsonPropertyName("weather")] List<WeatherDesc> Weather,
    [property: JsonPropertyName("main")] MainStats Main,
    [property: JsonPropertyName("wind")] WindStats Wind,
    [property: JsonPropertyName("clouds")] CloudStats Clouds,
    [property: JsonPropertyName("rain")] RainStats? Rain,
    [property: JsonPropertyName("snow")] SnowStats? Snow,
    [property: JsonPropertyName("visibility")] int Visibility,
    [property: JsonPropertyName("dt")] long UnixTime
);

public record WeatherDesc(string Main, string Description, string Icon);

public record MainStats(
    double Temp, 
    double Feels_Like, 
    double Temp_Min, 
    double Temp_Max, 
    int Pressure, 
    int Humidity, 
    int Sea_Level, 
    int Grnd_Level
);

public record WindStats(double Speed, int Deg);
public record CloudStats(int All);
public record RainStats([property: JsonPropertyName("1h")] double? OneHour);
public record SnowStats([property: JsonPropertyName("1h")] double? OneHour);