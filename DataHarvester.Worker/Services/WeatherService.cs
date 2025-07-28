using System.Net.Http.Json;
using DataHarvester.Worker.Models;

namespace DataHarvester.Worker.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, IConfiguration config, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _apiKey = config["Weather:ApiKey"] ?? throw new ArgumentException("Weather API key not found");
        _logger = logger;
    }

    public async Task<WeatherResult?>? FetchWeatherAsync(string city)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<WeatherResult>(
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={_apiKey}");
            return response;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching weather for {City}", city);
            return null;
        }
    }
}