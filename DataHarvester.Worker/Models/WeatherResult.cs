namespace DataHarvester.Worker.Models;

public class WeatherResult
{
    public MainData mainData { get; set; }
    public string Name {get; set;} = string.Empty;
}

public class MainData
{
    public float Temp { get; set; }
    public int Humidity { get; set; }
}