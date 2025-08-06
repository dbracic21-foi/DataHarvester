using DataHarvester.Infrastructure.ExternalApis;

namespace DataHarvester.Worker.Services;

public class WorkerService : BackgroundService
{
    private readonly IWeatherService _weatherService;

    public WorkerService(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var result = await _weatherService.FetchWeatherAsync("Zagreb")!;
        Console.WriteLine(result);
    }
}