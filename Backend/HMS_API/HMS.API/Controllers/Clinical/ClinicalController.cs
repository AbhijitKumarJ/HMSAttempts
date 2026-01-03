using HMS.Business.Clinical;
using HMS.Entity.Clinical;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers.Clinical;

[ApiController]
[Route("api/Clinical")]
public class ClinicalController : ControllerBase
{
    private readonly ILogger<ClinicalController> _logger;
    private readonly IClinicalService _clinicalService;

    public ClinicalController(ILogger<ClinicalController> logger, IClinicalService clinicalService)
    {
        _logger = logger;
        _clinicalService = clinicalService;
    }

    // GET: api/Clinical/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.GetUsers(limit));
    }

    // GET: api/Clinical/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.GetUser(id));
    }

    // POST: api/Clinical/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        return Ok(_clinicalService.CreateUser(entity));
    }

    // POST: api/Clinical/UpdateUser
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.UpdateUser(entity));
    }

    // GET: api/Clinical/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.DeleteUser(id));
    }
}
