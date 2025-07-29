using DataHarvester.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataHarvester.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<DataItem> DataItems => Set<DataItem>();
    public DbSet<DataSource> DataSources => Set<DataSource>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<City> Cities => Set<City>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.DataItemId });

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.DataItem)
            .WithMany(d => d.Favorites)
            .HasForeignKey(f => f.DataItemId);
        
        //Seed data 
        modelBuilder.Entity<DataSource>().HasData(
            new DataSource
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Crypto API",
                Type = "crypto"
            },
            new DataSource
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Weather API",
                Type = "weather"

            }
        );
    }
}