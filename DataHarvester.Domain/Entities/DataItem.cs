namespace DataHarvester.Domain.Entities;

public class DataItem
{
    public Guid Id { get; set; }
    public string? ExternalId { get; set; } = null!;
    public string? Title { get; set; } = null!;
    public string ContentJson { get; set; } = null!;
    public Guid? CityId { get; set; }
    public City? City { get; set; }

    public Guid SourceId { get; set; }
    public DataSource Source { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

}