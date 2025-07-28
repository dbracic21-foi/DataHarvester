using DataHarvester.Worker.Models;

namespace DataHarvester.Worker.Services;

public interface IWeatherService
{
    Task<WeatherResult?> FetchWeatherAsync(string city);

}