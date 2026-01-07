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

    // Story 3.2: Dynamic Form Builder API

    // POST: api/Clinical/templates
    [HttpPost("templates")]
    public async Task<IActionResult> CreateFormTemplate([FromBody] CreateFormTemplateDto dto, CancellationToken cancellationToken)
    {
        if (dto == null) return BadRequest();

        try
        {
            var result = _clinicalService.CreateFormTemplate(dto);
            return CreatedAtAction(nameof(GetFormTemplate), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating form template");
            return StatusCode(500, "Error creating form template");
        }
    }

    // GET: api/Clinical/templates/{id}
    [HttpGet("templates/{id:int}")]
    public async Task<IActionResult> GetFormTemplate(int id, CancellationToken cancellationToken)
    {
        var result = _clinicalService.GetFormTemplate(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // GET: api/Clinical/templates
    [HttpGet("templates")]
    public async Task<IActionResult> GetFormTemplates(CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.GetFormTemplates());
    }

    // DELETE: api/Clinical/templates/{id}
    [HttpDelete("templates/{id:int}")]
    public async Task<IActionResult> DeleteFormTemplate(int id, CancellationToken cancellationToken)
    {
        _clinicalService.DeleteFormTemplate(id);
        return NoContent();
    }

    // Story 3.3: Triage Workflow & Vitals Capture

    // POST: api/Clinical/vitals
    [HttpPost("vitals")]
    public async Task<IActionResult> CreateVital([FromBody] VitalsCaptureDto dto, CancellationToken cancellationToken)
    {
        if (dto == null || dto.PatientId == 0) return BadRequest("PatientId is required");

        try
        {
            var recordedBy = 1; 
            var result = _clinicalService.CreateVital(dto, recordedBy);
            return CreatedAtAction(nameof(GetVital), new { id = result.PatientId }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vital");
            return StatusCode(500, "Error creating vital");
        }
    }

    // GET: api/Clinical/vitals/{id}
    [HttpGet("vitals/{id:long}")]
    public async Task<IActionResult> GetVital(long id, CancellationToken cancellationToken)
    {
        var result = _clinicalService.GetVital(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // GET: api/Clinical/patients/{patientId}/vitals
    [HttpGet("patients/{patientId:int}/vitals")]
    public async Task<IActionResult> GetPatientVitals(int patientId, CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.GetPatientVitals(patientId));
    }

    // POST: api/Clinical/vitals/validate
    [HttpPost("vitals/validate")]
    public async Task<IActionResult> ValidateVitals([FromBody] VitalsCaptureDto dto, CancellationToken cancellationToken)
    {
        if (dto == null) return BadRequest();

        return Ok(_clinicalService.ValidateVitals(dto));
    }

    // Story 3.4: Vital Signs Alerting Logic

    // GET: api/Clinical/vitals/alert-rules
    [HttpGet("vitals/alert-rules")]
    public async Task<IActionResult> GetVitalsAlertRules(CancellationToken cancellationToken)
    {
        return Ok(_clinicalService.GetVitalsAlertRules());
    }
}
