using DataHarverster.Application.Services;
using DataHarvester.Infrastructure.ExternalApis;
using DataHarvester.Infrastructure.Persistence;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using DataHarvester.Infrastructure.Persistence.Repository;
using DataHarvester.Worker;
using DataHarvester.Worker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddHostedService<RabbitMqListenerService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<WeatherApiService>();
builder.Services.AddScoped<CryptoApiService>();
builder.Services.AddScoped<ExternalApiServiceFactory>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddHttpClient(); 
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
var host = builder.Build();
host.Run();