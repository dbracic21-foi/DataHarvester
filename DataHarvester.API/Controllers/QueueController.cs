using System.Text.Json;
using DataHarverster.Application.Interfaces;
using DataHarvester.API.Services;
using DataHarvester.Infrastructure.Persistence.Interfaces;
using DataHarvester.Shared.Queue;
using Microsoft.AspNetCore.Mvc;

namespace DataHarvester.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class QueueController : ControllerBase
{
    private readonly QueueSenderServices _queueSender;
    private readonly IWeatherDataService _weatherDataService;

    public QueueController(QueueSenderServices queueSender, IDataItemRepository dataItemRepository, IWeatherDataService weatherDataService)
    {
        _queueSender = queueSender;
        _weatherDataService = weatherDataService;
    }


    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] ApiFetchRequest request)
    {
        await _queueSender.SendAsync(request);
        return Ok("Sent async to queue.");
    }
    
    
    [HttpGet("weather/{city}")]
    public async Task<IActionResult> Send(string city)
    {
        var item = await _weatherDataService.GetLatestByCityAsync(city);
        if (item == null)
        {
            return BadRequest("City not found");
        }
        return Ok(JsonSerializer.Deserialize<object>(item.ContentJson));
    }
}