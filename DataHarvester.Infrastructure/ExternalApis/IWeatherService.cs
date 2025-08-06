using DataHarvester.Worker.Models;

namespace DataHarvester.Infrastructure.ExternalApis;

public interface IWeatherService
{
    Task<WeatherResult> FetchWeatherAsync(string city);

}