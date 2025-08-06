using DataHarvester.Domain.Entities;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataHarvester.Infrastructure.Persistence.Repository;

public class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetCityByNameAsync(string name) => await _context.Cities.FirstOrDefaultAsync(c => c.Name == name)!;

    public async Task AddCityAsync(City city) => await _context.Cities.AddAsync(city);

    public async Task UpdateCityAsync(City city) =>  _context.Cities.Update(city);

    public  async Task SaveChangesAsync() =>  await _context.SaveChangesAsync();
}