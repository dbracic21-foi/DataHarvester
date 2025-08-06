namespace DataHarvester.Shared.Queue;

public class ApiFetchRequest
{
    public Guid UserId { get; set; }            
    public string ApiType { get; set; }          
    public string Endpoint { get; set; }       
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public string? City {get; set;}
}