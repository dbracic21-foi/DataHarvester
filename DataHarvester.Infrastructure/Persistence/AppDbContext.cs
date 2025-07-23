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
    }
}