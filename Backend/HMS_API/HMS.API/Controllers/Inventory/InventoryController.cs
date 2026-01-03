using HMS.Business;
using HMS.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers;

[ApiController]
[Route("api/Inventory")]
public class InventoryController : ControllerBase
{
    private readonly ILogger<InventoryController> _logger;
    private readonly IUserService _userService;

    public InventoryController(ILogger<InventoryController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    // GET: api/TestPostgres/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(new JObject { ["Message"] = $"Retrieved {limit} users." });
    }

    // GET: api/TestPostgres/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_userService.GetUser(id));
    }

    // POST: api/TestPostgres/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        

        return CreatedAtAction(nameof(GetUser), new { id = 1 }, entity);
    }

    // POST: api/TestPostgres/UpdateUser/5
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(new JObject { ["Message"] = $"Updated user with ID {entity.UserId}." });
    }

    // GET: api/TestPostgres/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(new JObject { ["Message"] = $"Deleted user with ID {id}." });
    }
}
