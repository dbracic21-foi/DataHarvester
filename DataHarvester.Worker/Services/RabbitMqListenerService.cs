using System.Text;
using System.Text.Json;
using DataHarverster.Application.Services;
using DataHarvester.Shared.Queue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DataHarvester.Worker.Services;

public class RabbitMqListenerService : BackgroundService
{
    private readonly ILogger<RabbitMqListenerService> _logger;
    private readonly IConfiguration _config;
    private IConnection _connection;
    private IChannel _channel;
    private readonly IServiceProvider  _serviceProvider;

    
    public RabbitMqListenerService(ILogger<RabbitMqListenerService> logger, IConfiguration config, IServiceProvider services)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ config: Host={Host}, Port={Port}, Username={Username}, Password={Password}");
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQ:Host"],
            Port = int.Parse(_config["RabbitMQ:Port"] ?? string.Empty),
            UserName = _config["RabbitMQ:Username"],
            Password = _config["RabbitMQ:Password"]
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await _channel.QueueDeclareAsync("myQueue", true, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var factory = scope.ServiceProvider.GetRequiredService<ExternalApiServiceFactory>();
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var request = JsonSerializer.Deserialize<ApiFetchRequest>(json);
                if (request == null)
                {
                    _logger.LogWarning("[RabbitMQ] Invalid or null ApiFetchRequest received.");
                    return;
                }

                _logger.LogInformation($"[RabbitMQ] Received request from User: {request.UserId}, API Type: {request.ApiType}, Endpoint: {request.Endpoint}");
                var service = factory.GetService(request.ApiType);
                if (request.City != null) await service.FetchAndStoreAsync(request.City, stoppingToken);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RabbitMQ] Error processing message.");
            }

            await Task.CompletedTask;
        };

        
        await _channel.BasicConsumeAsync("myQueue", true, consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}