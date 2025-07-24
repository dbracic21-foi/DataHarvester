using System.Text;
using System.Text.Json;
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
    
    public RabbitMqListenerService(ILogger<RabbitMqListenerService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
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
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var request = JsonSerializer.Deserialize<ApiFetchRequest>(json);
                if (request == null)
                {
                    _logger.LogWarning("[RabbitMQ] Invalid or null ApiFetchRequest received.");
                    return;
                }

                _logger.LogInformation($"[RabbitMQ] Received request from User: {request.UserId}, API Type: {request.ApiType}, Endpoint: {request.Endpoint}");
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(request.Endpoint);
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"[API] Status: {response.StatusCode}, Body (trimmed): {responseBody[..Math.Min(200, responseBody.Length)]}");
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