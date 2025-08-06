namespace DataHarvester.Infrastructure.Services.Interfaces;

public interface IQueueSenderService
{
    Task SendFetchRequestAsync(string cityName, CancellationToken cancellationToken = default);

    
}