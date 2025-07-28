using DataHarverster.Application.Services;
using DataHarvester.Infrastructure.ExternalApis;
using DataHarvester.Worker;
using DataHarvester.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
Console.WriteLine("==== Loaded Configuration ====");

builder.Services.AddHostedService<RabbitMqListenerService>();
builder.Services.AddScoped<WeatherApiService>();
builder.Services.AddScoped<CryptoApiService>();
builder.Services.AddScoped<ExternalApiServiceFactory>();
//builder.Services.AddHttpClient(); 
//builder.Services.AddHttpClient<IWeatherService, WeatherService>();
var host = builder.Build();
host.Run();