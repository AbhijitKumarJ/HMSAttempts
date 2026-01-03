using HMS.Business.Cpoe;
using HMS.Entity.Cpoe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers.Cpoe;

[ApiController]
[Route("api/Cpoe")]
public class CpoeController : ControllerBase
{
    private readonly ILogger<CpoeController> _logger;
    private readonly ICpoeService _cpoeService;

    public CpoeController(ILogger<CpoeController> logger, ICpoeService cpoeService)
    {
        _logger = logger;
        _cpoeService = cpoeService;
    }

    // GET: api/Cpoe/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(_cpoeService.GetUsers(limit));
    }

    // GET: api/Cpoe/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_cpoeService.GetUser(id));
    }

    // POST: api/Cpoe/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        return Ok(_cpoeService.CreateUser(entity));
    }

    // POST: api/Cpoe/UpdateUser
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(_cpoeService.UpdateUser(entity));
    }

    // GET: api/Cpoe/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_cpoeService.DeleteUser(id));
    }
}
