using HMS.Business;
using HMS.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HMS.API.Controllers;

[ApiController]
[Route("api/TestPostgres")]
public class TestPostgresController : ControllerBase
{
    private readonly ILogger<TestPostgresController> _logger;
    private readonly ICustomerService _customerService;

    public TestPostgresController(ILogger<TestPostgresController> logger, ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    // GET: api/TestPostgres/GetCustomers
    [HttpGet("GetCustomers")]
    public async Task<IActionResult> GetCustomers([FromQuery]int limit,CancellationToken cancellationToken)
    {
        return Ok(new JObject { ["Message"] = $"Retrieved {limit} customers." });
    }

    // GET: api/TestPostgres/GetCustomer/5
    [HttpGet("GetCustomer/{id:int}")]
    public async Task<IActionResult> GetCustomer(int id, CancellationToken cancellationToken)
    {
        return Ok(_customerService.GetCustomer(id));
    }

    // POST: api/TestPostgres/CreateCustomer
    [HttpPost("CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerEntity entity, CancellationToken cancellationToken)
    {
        if (entity == null) return BadRequest();

        

        return CreatedAtAction(nameof(GetCustomer), new { id = 1 }, entity);
    }

    // POST: api/TestPostgres/UpdateCustomer/5
    [HttpPost("UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerEntity entity, CancellationToken cancellationToken)
    {
        return Ok(new JObject { ["Message"] = $"Updated customer with ID {entity.CustomerId}." });
    }

    // GET: api/TestPostgres/DeleteCustomer/5
    [HttpGet("DeleteCustomer/{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
    {
        return Ok(new JObject { ["Message"] = $"Deleted customer with ID {id}." });
    }
}
