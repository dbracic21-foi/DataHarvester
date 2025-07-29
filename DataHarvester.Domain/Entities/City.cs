namespace DataHarvester.Domain.Entities;

public class City
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<DataItem> DataItems { get; set; } = new List<DataItem>();
}