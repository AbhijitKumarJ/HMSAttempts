using HMS.Business.Billing;
using HMS.Entity.Billing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers.Billing;

[ApiController]
[Route("api/Billing")]
public class BillingController : ControllerBase
{
    private readonly ILogger<BillingController> _logger;
    private readonly IBillingService _billingService;

    public BillingController(ILogger<BillingController> logger, IBillingService billingService)
    {
        _logger = logger;
        _billingService = billingService;
    }

    // GET: api/Billing/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(_billingService.GetUsers(limit));
    }

    // GET: api/Billing/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_billingService.GetUser(id));
    }

    // POST: api/Billing/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        return CreatedAtAction(nameof(GetUser), new { id = 1 }, _billingService.CreateUser(entity));
    }

    // POST: api/Billing/UpdateUser
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(_billingService.UpdateUser(entity));
    }

    // GET: api/Billing/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_billingService.DeleteUser(id));
    }
}
