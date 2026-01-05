using HMS.Data.Patient;
using HMS.Entity.Patient;
using HMS.Data.DBModel;
using HMS.Bus;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Patient;

public interface IPatientService
{
    Task<PatientResponseDto> GetPatientByMrn(string mrn);
    Task<List<PatientSearchResultDto>> SearchPatientsAsync(string query);
    Task<List<PatientSearchResultDto>> GetAllPatientsAsync();
    Task<PatientResponseDto> RegisterEmergencyAsync(EmergencyRegistrationDto dto, int userId);
    Task<PatientResponseDto?> UpdatePatientAsync(string mrn, UpdatePatientDto dto, int userId);
}

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMrnGenerator _mrnGenerator;
    private readonly IEventBus _eventBus;
    private readonly HMSContext _context;

    public PatientService(
        IPatientRepository patientRepository,
        IMrnGenerator mrnGenerator,
        IEventBus eventBus,
        HMSContext context)
    {
        _patientRepository = patientRepository;
        _mrnGenerator = mrnGenerator;
        _eventBus = eventBus;
        _context = context;
    }
  

    public async Task<PatientResponseDto> RegisterEmergencyAsync(EmergencyRegistrationDto dto, int userId)
    {
        var mrn = await _mrnGenerator.GenerateMrnAsync();

        var patient = new PatPatient
        {
            Mrn = mrn,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            IsEmergencyReg = true,
            CreatedAt = DateTime.UtcNow
        };

        var createdPatient = await _patientRepository.CreateAsync(patient);

        await _eventBus.PublishAsync("Patient.Created", new
        {
            PatientId = createdPatient.Id,
            Mrn = createdPatient.Mrn,
            FullName = $"{createdPatient.FirstName} {createdPatient.LastName}",
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow
        });

        return new PatientResponseDto
        {
            Id = createdPatient.Id,
            Mrn = createdPatient.Mrn!,
            FirstName = createdPatient.FirstName!,
            LastName = createdPatient.LastName!,
            Gender = createdPatient.Gender,
            Dob = createdPatient.Dob,
            IsEmergencyReg = createdPatient.IsEmergencyReg ?? false,
            CreatedAt = createdPatient.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<PatientResponseDto?> UpdatePatientAsync(string mrn, UpdatePatientDto dto, int userId)
    {
        var patient = await _patientRepository.GetByMrnAsync(mrn);
        if (patient == null)
        {
            return null;
        }

        var oldValues = new
        {
            patient.FirstName,
            patient.LastName,
            patient.Dob,
            patient.Gender,
            patient.ContactInfo
        };

        if (dto.FirstName != null)
            patient.FirstName = dto.FirstName;
        if (dto.LastName != null)
            patient.LastName = dto.LastName;
        if (dto.Dob != null)
            patient.Dob = dto.Dob.Value;
        if (dto.Gender != null)
            patient.Gender = dto.Gender;

        if (dto.ContactInfo != null)
        {
            patient.ContactInfo = Newtonsoft.Json.JsonConvert.SerializeObject(dto.ContactInfo);
        }

        patient.IsEmergencyReg = false;

        var updatedPatient = await _patientRepository.UpdateAsync(patient);

        var newValues = new
        {
            updatedPatient.FirstName,
            updatedPatient.LastName,
            updatedPatient.Dob,
            updatedPatient.Gender,
            updatedPatient.ContactInfo
        };

        var auditLog = new AuditLog
        {
            EntityType = "Patient",
            EntityId = mrn,
            Action = "Update",
            OldValue = Newtonsoft.Json.JsonConvert.SerializeObject(oldValues),
            NewValue = Newtonsoft.Json.JsonConvert.SerializeObject(newValues),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();

        ContactInfoDto? contactInfo = null;
        if (!string.IsNullOrEmpty(updatedPatient.ContactInfo))
        {
            contactInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactInfoDto>(updatedPatient.ContactInfo);
        }

        return new PatientResponseDto
        {
            Id = updatedPatient.Id,
            Mrn = updatedPatient.Mrn!,
            FirstName = updatedPatient.FirstName!,
            LastName = updatedPatient.LastName!,
            Gender = updatedPatient.Gender,
            Dob = updatedPatient.Dob,
            ContactInfo = contactInfo,
            IsEmergencyReg = updatedPatient.IsEmergencyReg ?? false,
            CreatedAt = updatedPatient.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<PatientResponseDto> GetPatientByMrn(string mrn)
    {
        var patient = await _patientRepository.GetByMrnAsync(mrn);
        if (patient == null)
        {
            return null;
        }

        ContactInfoDto? contactInfo = null;
        if (!string.IsNullOrEmpty(patient.ContactInfo))
        {
            contactInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<ContactInfoDto>(patient.ContactInfo);
        }

        return new PatientResponseDto
        {
            Id = patient.Id,
            Mrn = patient.Mrn!,
            FirstName = patient.FirstName!,
            LastName = patient.LastName!,
            Gender = patient.Gender,
            Dob = patient.Dob,
            ContactInfo = contactInfo,
            IsEmergencyReg = patient.IsEmergencyReg ?? false,
            CreatedAt = patient.CreatedAt ?? DateTime.UtcNow
        };
    }

    public async Task<List<PatientSearchResultDto>> SearchPatientsAsync(string query)
    {
        var patients = await _patientRepository.SearchPatientsAsync(query);
        
        return patients.Select(p => new PatientSearchResultDto
        {
            Id = p.Id,
            Mrn = p.Mrn!,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Gender = p.Gender,
            Dob = p.Dob
        }).ToList();
    }

    public async Task<List<PatientSearchResultDto>> GetAllPatientsAsync()
    {
        var patients = await _patientRepository.GetAllPatientsAsync();
        
        return patients.Select(p => new PatientSearchResultDto
        {
            Id = p.Id,
            Mrn = p.Mrn!,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Gender = p.Gender,
            Dob = p.Dob
        }).ToList();
    }
}