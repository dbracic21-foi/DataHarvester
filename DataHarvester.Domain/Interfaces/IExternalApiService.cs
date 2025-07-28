namespace DataHarvester.Domain.Inrefaces;

public interface IExternalApiService
{
    Task FetchAndStoreAsync(string endpoint, CancellationToken cancellationToken);

}