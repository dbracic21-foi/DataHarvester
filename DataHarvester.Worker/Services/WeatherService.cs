using DataHarvester.Worker.Models;

namespace DataHarvester.Worker.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, string apiKey, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        _logger = logger;
    }

    public Task<WeatherResult?> FetchWeatherAsync(string city)
    {
        throw new NotImplementedException();
    }
}