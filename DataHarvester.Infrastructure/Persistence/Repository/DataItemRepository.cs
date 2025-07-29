using System.Data;
using DataHarvester.Domain.Entities;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataHarvester.Infrastructure.Persistence.Repository;

public class DataItemRepository : IDataItemRepository
{
    private readonly AppDbContext _dbContext;

    public DataItemRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DataItem?> GetLatestByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DataItems
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(x => x.Title != null && x.Title.ToLower().Contains(city.ToLower()), cancellationToken);
    }
}