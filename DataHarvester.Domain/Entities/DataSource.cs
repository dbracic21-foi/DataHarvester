namespace DataHarvester.Domain.Entities;

public class DataSource
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public ICollection<DataItem> DataItems { get; set; } = new List<DataItem>();
}