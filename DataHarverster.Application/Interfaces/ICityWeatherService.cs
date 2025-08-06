using DataHarvester.Domain.Entities;

namespace DataHarverster.Application.Interfaces;

public interface ICityWeatherService
{
    Task<DataItem?> GetOrFetchCityWeatherDataAsync(string city, CancellationToken cancellationToken);
}