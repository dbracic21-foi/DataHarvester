using System.Text;
using System.Text.Json;
using DataHarvester.Infrastructure.Services.Interfaces;
using DataHarvester.Shared.Queue;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace DataHarvester.Infrastructure.Services;

public class QueueSenderService : IQueueSenderService
{
    private readonly IConfiguration _configuration;
    private  IConnection? _connection;
    private IChannel? _channel;
    private readonly ConnectionFactory _factory;

    public QueueSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        _factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQ:Host"],
            Port = int.Parse(_configuration["RabbitMQ:Port"]),
            UserName = _configuration["RabbitMQ:Username"],
            Password = _configuration["RabbitMQ:Password"]
        };
    }

    public async Task SendFetchRequestAsync(string cityName, CancellationToken cancellationToken = default)
    {
        _connection ??= await _factory.CreateConnectionAsync(cancellationToken);
        _channel ??= await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
    
        await _channel.QueueDeclareAsync("myQueue", true, false, false, null, cancellationToken: cancellationToken);

        var request = new ApiFetchRequest
        {
            ApiType = "weather",
            City = cityName,
            Endpoint = $"/weather/{cityName}", 
            RequestedAt = DateTime.UtcNow,
            UserId = Guid.Empty //Right now when we dont have implemented user  is empty
        };

        var json = JsonSerializer.Serialize(request);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: "myQueue",
            body: body);
    }

}