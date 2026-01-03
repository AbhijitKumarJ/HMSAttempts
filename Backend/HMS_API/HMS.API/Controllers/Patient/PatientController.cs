using HMS.Business.Patient;
using HMS.Entity.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers.Patient;

[ApiController]
[Route("api/Patient")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly IPatientService _patientService;

    public PatientController(ILogger<PatientController> logger, IPatientService patientService)
    {
        _logger = logger;
        _patientService = patientService;
    }

    // GET: api/Patient/GetUsers
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(_patientService.GetUsers(limit));
    }

    // GET: api/Patient/GetUser/5
    [HttpGet("GetUser/{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_patientService.GetUser(id));
    }

    // POST: api/Patient/CreateUser
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        return Ok(_patientService.CreateUser(entity));
    }

    // POST: api/Patient/UpdateUser
    [HttpPost("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserEntity entity, CancellationToken cancellationToken)
    {
        return Ok(_patientService.UpdateUser(entity));
    }

    // GET: api/Patient/DeleteUser/5
    [HttpGet("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        return Ok(_patientService.DeleteUser(id));
    }
}
