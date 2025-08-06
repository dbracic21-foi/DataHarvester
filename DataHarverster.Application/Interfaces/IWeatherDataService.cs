using DataHarvester.Domain.Entities;

namespace DataHarverster.Application.Interfaces;

public interface IWeatherDataService
{
    Task<DataItem?> GetLatestByCityAsync(string city, CancellationToken cancellationToken = default);
    Task<DataItem?> TriggerAndFetchCityAsync(string city, CancellationToken cancellationToken = default);

}