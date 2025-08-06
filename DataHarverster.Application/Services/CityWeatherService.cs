using DataHarverster.Application.Interfaces;
using DataHarvester.Domain.Entities;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using Microsoft.Extensions.Logging;

namespace DataHarverster.Application.Services;

public class CityWeatherService : ICityWeatherService
{
    //private readonly ICityRepository _cityRepository;
    private readonly IDataItemRepository _dataItemRepository;
    //private readonly IExternalApiFetcher _apiFetcher; // servis koji fetcha i sprema novi podatak
    private readonly ILogger<CityWeatherService> _logger;

    public CityWeatherService(IDataItemRepository dataItemRepository, ILogger<CityWeatherService> logger)
    {
        _dataItemRepository = dataItemRepository;
        _logger = logger;
    }

    public Task<DataItem?> GetOrFetchCityWeatherDataAsync(string city, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}