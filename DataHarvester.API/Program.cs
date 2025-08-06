using DataHarverster.Application.Interfaces;
using DataHarverster.Application.Services;
using DataHarvester.API.Services;
using DataHarvester.Infrastructure.ExternalApis;
using DataHarvester.Infrastructure.Persistence;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using DataHarvester.Infrastructure.Persistence.Repository;
using DataHarvester.Infrastructure.Services;
using DataHarvester.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton<QueueSenderServices>();
builder.Services.AddControllers();
builder.Services.AddScoped<IDataItemRepository, DataItemRepository>();
builder.Services.AddScoped<IWeatherDataService, WeatherDataService>();
builder.Services.AddScoped<IQueueSenderService, QueueSenderService>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
