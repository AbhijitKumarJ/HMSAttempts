using HMS.Business.Patient;
using HMS.Entity.Patient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace HMS.API.Controllers.Patient;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;
    private readonly IPatientService _patientService;

    public PatientController(ILogger<PatientController> logger, IPatientService patientService)
    {
        _logger = logger;
        _patientService = patientService;
    }

    private int? GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        return null;
    }

    
    // POST: api/patients/emergency
    [HttpPost("emergency")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EmergencyRegistration(
        [FromBody] EmergencyRegistrationDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserIdFromToken();
        if (!userId.HasValue)
        {
            return Unauthorized(new { error = new { code = "UNAUTHORIZED", message = "User ID not found in token" } });
        }

        try
        {
            var result = await _patientService.RegisterEmergencyAsync(dto, userId.Value);
            return CreatedAtAction(nameof(GetPatientByMrn), new { mrn = result.Mrn }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during emergency registration");
            return StatusCode(500, new { error = new { code = "INTERNAL_ERROR", message = "An error occurred while registering patient" } });
        }
    }

    // POST: api/patients/{mrn}
    [HttpPost("{mrn}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePatient(
        string mrn,
        [FromBody] UpdatePatientDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = GetUserIdFromToken();
        if (!userId.HasValue)
        {
            return Unauthorized(new { error = new { code = "UNAUTHORIZED", message = "User ID not found in token" } });
        }

        try
        {
            var result = await _patientService.UpdatePatientAsync(mrn, dto, userId.Value);
            if (result == null)
            {
                return NotFound(new { error = new { code = "PATIENT_NOT_FOUND", message = $"Patient with MRN {mrn} not found" } });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient {Mrn}", mrn);
            return StatusCode(500, new { error = new { code = "INTERNAL_ERROR", message = "An error occurred while updating patient" } });
        }
    }

    // GET: api/patients/{mrn}
    [HttpGet("{mrn}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientByMrn(string mrn, CancellationToken cancellationToken)
    {
        var result = await _patientService.GetPatientByMrn(mrn);
        if (result == null)
        {
            return NotFound(new { error = new { code = "PATIENT_NOT_FOUND", message = $"Patient with MRN {mrn} not found" } });
        }
        return Ok(new { message = $"Patient with MRN {mrn} retrieved.", data = result });
    }
}
