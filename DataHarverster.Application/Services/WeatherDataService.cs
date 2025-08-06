using DataHarverster.Application.Interfaces;
using DataHarvester.Domain.Entities;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using DataHarvester.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DataHarverster.Application.Services;

public class WeatherDataService : IWeatherDataService
{
    public const int maxRetries = 10;
    public const int delayMs = 500;
    
    
    private readonly IDataItemRepository _dataItemRepository;
    private readonly ILogger<WeatherDataService> _logger;
    private readonly IQueueSenderService _queueSender;


    public WeatherDataService(IDataItemRepository dataItemRepository, ILogger<WeatherDataService> logger, IQueueSenderService queueSender)
    {
        _dataItemRepository = dataItemRepository;
        _logger = logger;
        _queueSender = queueSender;
    }

    public async Task<DataItem?> GetLatestByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching latest data for {City}", city);
        var item = await _dataItemRepository.GetLatestByCityAsync(city, cancellationToken);

        if (item == null)
        {
            _logger.LogInformation("No data found for {City}, fetching new data...", city);
            await TriggerAndFetchCityAsync(city, cancellationToken);
            
            for (int i = 0; i < maxRetries; i++)
            {
                await Task.Delay(delayMs, cancellationToken);
                item = await _dataItemRepository.GetLatestByCityAsync(city, cancellationToken);
                if (item != null)
                {
                    break;
                }
            }
            
            if (item == null)
            {
                _logger.LogWarning("Still no data found for {City} after fetch attempt.", city);
                return null;
            }
        }

        var isOlderThan15Min = DateTime.UtcNow - item.CreatedAt > TimeSpan.FromMinutes(15);

        if (isOlderThan15Min) _logger.LogInformation("Data for {City} is older than 15 min, refreshing...", city);
        await TriggerAndFetchCityAsync(city, cancellationToken);
        return item;
    }

    public async Task<DataItem?> TriggerAndFetchCityAsync(string city, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Triggering fetch for city: {city}", city);

        await _queueSender.SendFetchRequestAsync(city, cancellationToken);

        await Task.Delay(5000, cancellationToken);

        var item = await _dataItemRepository.GetLatestByCityAsync(city, cancellationToken);

        return item;
    }
}