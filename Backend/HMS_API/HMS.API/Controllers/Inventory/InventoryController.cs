using HMS.Business.Inventory;
using HMS.Entity.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers.Inventory;

[ApiController]
[Route("api/Inventory")]
public class InventoryController : ControllerBase
{
    private readonly ILogger<InventoryController> _logger;
    private readonly IInventoryService _inventoryService;

    public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    // GET: api/Inventory/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(_inventoryService.GetUsers(limit));
    }

    // GET: api/Inventory/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_inventoryService.GetUser(id));
    }

    // POST: api/Inventory/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        return Ok(_inventoryService.CreateUser(entity));
    }

    // POST: api/Inventory/UpdateUser
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(_inventoryService.UpdateUser(entity));
    }

    // GET: api/Inventory/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_inventoryService.DeleteUser(id));
    }
}
