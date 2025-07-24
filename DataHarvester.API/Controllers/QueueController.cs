using DataHarvester.API.Services;
using DataHarvester.Shared.Queue;
using Microsoft.AspNetCore.Mvc;

namespace DataHarvester.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class QueueController : ControllerBase
{
    private readonly QueueSenderServices _queueSender;

    public QueueController(QueueSenderServices queueSender)
    {
        _queueSender = queueSender;
    }


    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] ApiFetchRequest request)
    {
        await _queueSender.SendAsync(request);
        return Ok("Sent async to queue.");
    }
}