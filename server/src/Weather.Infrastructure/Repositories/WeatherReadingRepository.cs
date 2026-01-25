using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Persistence;
using Weather.Application.Common.DTOs;

namespace Weather.Infrastructure.Repositories;

public class WeatherReadingRepository : GenericRepository<WeatherReading>, IWeatherReadingRepository
{
    public WeatherReadingRepository(AppDbContext context) : base(context) { }

}