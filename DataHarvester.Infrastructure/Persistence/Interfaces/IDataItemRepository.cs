using DataHarvester.Domain.Entities;

namespace DataHarvester.Infrastructure.Persistence.Interfaces;

public interface IDataItemRepository
{
    Task<DataItem?> GetLatestByCityAsync(string city, CancellationToken cancellationToken = default);

}