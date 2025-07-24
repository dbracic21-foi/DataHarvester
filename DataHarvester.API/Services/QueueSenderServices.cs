using System.Text;
using System.Text.Json;
using DataHarvester.Shared.Queue;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace DataHarvester.API.Services;

public class QueueSenderServices : IAsyncDisposable
{
    private readonly IConfiguration _configuration;
    private  IConnection? _connection;
    private IChannel? _channel;
    private readonly ConnectionFactory _factory;

    public QueueSenderServices(IConfiguration configuration)
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

    public async Task SendAsync(ApiFetchRequest request, CancellationToken token = default)
    {
        _connection ??= await _factory.CreateConnectionAsync(token);
        _channel ??= await _connection.CreateChannelAsync(cancellationToken: token);
        
        await _channel.QueueDeclareAsync("myQueue", true, false, false, null);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: "myQueue",
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.DisposeAsync();
            
        }
    }
}