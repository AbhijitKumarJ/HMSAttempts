using HMS.Business.Scheduling;
using HMS.Entity.Scheduling;
using Microsoft.AspNetCore.Mvc;

namespace HMS.API.Controllers.Scheduling;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
    private readonly ILogger<AppointmentController> _logger;
    private readonly ISchedulingService _schedulingService;

    public AppointmentController(ILogger<AppointmentController> logger, ISchedulingService schedulingService)
    {
        _logger = logger;
        _schedulingService = schedulingService;
    }

    // GET: api/appointments
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAppointments(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? doctorId,
        CancellationToken cancellationToken)
    {
        var searchStartDate = startDate ?? DateTime.Today;

        try
        {
            var result = await _schedulingService.SearchAppointmentsAsync(searchStartDate, endDate, doctorId);
            return Ok(new { message = "Appointments retrieved.", data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching appointments");
            return StatusCode(500, new { error = new { code = "INTERNAL_ERROR", message = "An error occurred while searching appointments" } });
        }
    }

    // POST: api/appointments
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BookAppointment(
        [FromBody] BookAppointmentDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _schedulingService.BookAppointmentAsync(dto);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = new { code = "VALIDATION_ERROR", message = ex.Message } });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = new { code = "DOUBLE_BOOKING", message = ex.Message } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error booking appointment");
            return StatusCode(500, new { error = new { code = "INTERNAL_ERROR", message = "An error occurred while booking appointment" } });
        }
    }

    // GET: api/appointments/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppointmentById(long id, CancellationToken cancellationToken)
    {
        var result = await _schedulingService.GetAppointmentByIdAsync(id);
        if (result == null)
        {
            return NotFound(new { error = new { code = "APPOINTMENT_NOT_FOUND", message = $"Appointment with ID {id} not found" } });
        }
        return Ok(new { message = $"Appointment with ID {id} retrieved.", data = result });
    }
}
