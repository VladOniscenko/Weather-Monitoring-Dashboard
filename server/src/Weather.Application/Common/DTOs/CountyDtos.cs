namespace Weather.Application.Common.DTOs;

public record CountryDto(
    Guid Id,
    string Name,
    string CCA2,
    string CCA3,
    string Region,
    string Subregion,
    string Capital,
    string Flag,
    double Latitude,
    double Longitude,
    bool Independent,
    bool Landlocked
);

public record CreateCountryRequest(
    string Name, 
    string CCA2, 
    string CCA3, 
    string Region, 
    string Subregion, 
    string Capital, 
    string Flag, 
    double Latitude, 
    double Longitude, 
    bool Independent, 
    bool Landlocked
);

public record UpdateCountryRequest(
    string Name,
    string CCA2,
    string CCA3,
    string Region,
    string Subregion,
    string Capital,
    string Flag,
    double Latitude,
    double Longitude,
    bool Independent,
    bool Landlocked
);