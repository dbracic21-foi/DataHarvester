using DataHarvester.Domain.Inrefaces;
using DataHarvester.Infrastructure.ExternalApis;
using Microsoft.Extensions.DependencyInjection;

namespace DataHarverster.Application.Services;

public class ExternalApiServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ExternalApiServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExternalApiService GetService(string apiType)
    {
        return apiType.ToLower() switch
        {
            "weather" => _serviceProvider.GetRequiredService<WeatherApiService>(),
            "crypto" => _serviceProvider.GetRequiredService<CryptoApiService>(),
            _ => throw new ArgumentException("Unsupported API type")
        };
    }
}