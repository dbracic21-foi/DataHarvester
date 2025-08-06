using System.Text.Json;
using DataHarvester.Domain.Entities;
using DataHarvester.Domain.Inrefaces;
using DataHarvester.Infrastructure.Persistence;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataHarvester.Infrastructure.ExternalApis;

public class WeatherApiService : IExternalApiService
{
    public const string WeatherApi = "weather";
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbContext;
    private readonly IWeatherService _weatherService;
    private readonly ICityRepository _cityRepository;
    public WeatherApiService(HttpClient httpClient, AppDbContext dbContext, IConfiguration config, IWeatherService weatherService, ICityRepository cityRepository)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
        _weatherService = weatherService;
        _cityRepository = cityRepository;
    }

    public async Task FetchAndStoreAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await _weatherService.FetchWeatherAsync(endpoint)!;

        var dataSource =
            await _dbContext.DataSources.FirstOrDefaultAsync(dt => dt.Type.ToLower() == WeatherApi, cancellationToken);
        
            var city = await _cityRepository.GetCityByNameAsync(response.Name);
            if (city == null)
            {
                city = new City()
                {
                    Name = response.Name,
                    Country = response.Sys.Country,
                    CreatedAt = DateTime.UtcNow,
                };
                await _cityRepository.AddCityAsync(city);
                await _cityRepository.SaveChangesAsync();
            }
        

        if (dataSource == null) return;
        var item = new DataItem
        {
            Id = Guid.NewGuid(),
            Title = response?.Name, 
            ExternalId = response.Id.ToString(),
            ContentJson = JsonSerializer.Serialize(response),
            SourceId = dataSource.Id,
            CreatedAt = DateTime.UtcNow,
            CityId = city.Id
        };

        _dbContext.DataItems.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}