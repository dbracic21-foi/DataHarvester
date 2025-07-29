using DataHarvester.Domain.Entities;
using DataHarvester.Domain.Inrefaces;
using DataHarvester.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DataHarvester.Infrastructure.ExternalApis;

public class CryptoApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbContext;

    public CryptoApiService(HttpClient httpClient, AppDbContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    public async Task FetchAndStoreAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return;

        var dataSource =
            await _dbContext.DataSources.FirstOrDefaultAsync(dt => dt.Type.ToLower() == "crypto", cancellationToken);
        if (dataSource == null) return;
        //Todo : Throw some error here
        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        var item = new DataItem
        {
            Id = Guid.NewGuid(),
            Title = "Crypto API",
            ContentJson = json,
            SourceId = dataSource.Id,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.DataItems.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}