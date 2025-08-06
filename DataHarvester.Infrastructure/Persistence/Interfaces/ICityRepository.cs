using DataHarvester.Domain.Entities;

namespace DataHarvester.Infrastructure.Persistence.Interfaces;

public interface ICityRepository
{
    Task<City?> GetCityByNameAsync(string name);
    Task AddCityAsync(City city);
    Task UpdateCityAsync(City city);
    Task SaveChangesAsync();
}