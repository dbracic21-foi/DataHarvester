namespace DataHarvester.Domain.Entities;

public class Favorite
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid DataItemId { get; set; }
    public DataItem DataItem { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}