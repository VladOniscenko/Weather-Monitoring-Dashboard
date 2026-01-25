using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Persistence;
using Weather.Application.Common.DTOs;

namespace Weather.Infrastructure.Repositories;

public class WeatherStationRepository : GenericRepository<WeatherStation>, IWeatherStationRepository
{
    public WeatherStationRepository(AppDbContext context) : base(context) { }

}