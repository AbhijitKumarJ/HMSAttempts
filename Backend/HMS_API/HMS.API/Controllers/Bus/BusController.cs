using HMS.Bus;
using HMS.Business.Bus;
using HMS.Entity.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers.Bus;

[ApiController]
[Route("api/Bus")]
public class BusController : ControllerBase
{
    private readonly ILogger<BusController> _logger;
    private readonly IBusService _busService;
    private readonly IEventBus _eventBus;

    public BusController(ILogger<BusController> logger, IBusService busService, IEventBus eventBus)
    {
        _logger = logger;
        _busService = busService;
        _eventBus = eventBus;
    }

    // GET: api/Bus/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(_busService.GetUsers(limit));
    }

    // GET: api/Bus/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_busService.GetUser(id));
    }

    // POST: api/Bus/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        return CreatedAtAction(nameof(GetUser), new { id = 1 }, _busService.CreateUser(entity));
    }

    // POST: api/Bus/UpdateUser
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(_busService.UpdateUser(entity));
    }

    // GET: api/Bus/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_busService.DeleteUser(id));
    }

    // EventBus Endpoints

    // POST: api/Bus/events/publish
    [HttpPost("events/publish")]
    public async Task<IActionResult> PublishEvent([FromBody] EventPayload payload, CancellationToken cancellationToken)
    {
        if (payload == null) return BadRequest();

        var eventId = await _eventBus.PublishAsync(payload.EventType, payload.Data);
        return CreatedAtAction(nameof(FetchEvent), new { eventType = payload.EventType }, new { id = eventId, message = "Event published successfully" });
    }

    // GET: api/Bus/events/fetch
    [HttpGet("events/fetch")]
    public async Task<IActionResult> FetchEvent([FromQuery] string eventType, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(eventType)) return BadRequest("eventType is required");

        var result = await _eventBus.FetchPendingAsync(eventType);
        if (result == null)
        {
            return Ok(new { message = "No pending events found" });
        }

        var payload = result.Value.Payload;
        var deserializedPayload = string.IsNullOrEmpty(payload) ? null : JToken.Parse(payload);

        return Ok(new
        {
            id = result.Value.Id,
            eventType = eventType,
            payload = deserializedPayload
        });
    }

    // POST: api/Bus/events/complete
    [HttpPost("events/complete")]
    public async Task<IActionResult> CompleteEvent([FromBody] Guid eventId, CancellationToken cancellationToken)
    {
        if (eventId == Guid.Empty) return BadRequest("eventId is required");

        await _eventBus.MarkCompletedAsync(eventId);
        return Ok(new { message = $"Event {eventId} marked as completed" });
    }

    // POST: api/Bus/events/fail
    [HttpPost("events/fail")]
    public async Task<IActionResult> FailEvent([FromBody] Guid eventId, CancellationToken cancellationToken)
    {
        if (eventId == Guid.Empty) return BadRequest("eventId is required");

        await _eventBus.MarkFailedAsync(eventId);
        return Ok(new { message = $"Event {eventId} marked as failed" });
    }
}
