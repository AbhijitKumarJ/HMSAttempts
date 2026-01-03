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

    public BusController(ILogger<BusController> logger, IBusService busService)
    {
        _logger = logger;
        _busService = busService;
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
}
