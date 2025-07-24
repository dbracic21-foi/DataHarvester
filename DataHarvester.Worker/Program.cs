using DataHarvester.Worker;
using DataHarvester.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
Console.WriteLine("==== Loaded Configuration ====");
foreach (var kvp in builder.Configuration.AsEnumerable())
{
    Console.WriteLine($"{kvp.Key} = {kvp.Value}");
}
Console.WriteLine("================================");
builder.Services.AddHostedService<RabbitMqListenerService>();

var host = builder.Build();
host.Run();