using DataHarverster.Application.Interfaces;
using DataHarvester.Domain.Entities;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace DataHarverster.Application.Services;

public class WeatherDataService : IWeatherDataService
{
    private readonly IDataItemRepository _dataItemRepository;
    private readonly ILogger<WeatherDataService> _logger;

    public WeatherDataService(IDataItemRepository dataItemRepository, ILogger<WeatherDataService> logger)
    {
        _dataItemRepository = dataItemRepository;
        _logger = logger;
    }

    public async Task<DataItem?> GetLatestByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching latest data for {City}", city);
        var item = await _dataItemRepository.GetLatestByCityAsync(city, cancellationToken);

        if (item == null)
        {
            _logger.LogInformation("No data found for {City}, fetching new data...", city);
            //Todo : Implement TrigerDataFetch
            //await TriggerDataFetch(city, cancellationToken);
            return null;
        }

        var isOlderThan15Min = DateTime.UtcNow - item.CreatedAt > TimeSpan.FromMinutes(15);

        if (isOlderThan15Min) _logger.LogInformation("Data for {City} is older than 15 min, refreshing...", city);
        //Todo : Implement TrigerDataFetch
        //await TriggerDataFetch(city, cancellationToken);
        return item;
    }
}