using HMS.Data.DBModel;
using HMS.Entity.Clinical;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Clinical;

public interface IClinicalRepository
{
    object GetUserById(int id);

    FormTemplateDto CreateFormTemplate(CreateFormTemplateDto dto);
    FormTemplateDto? GetFormTemplate(int id);
    List<FormTemplateDto> GetFormTemplates();
    void DeleteFormTemplate(int id);

    ClinVital CreateVital(VitalsCaptureDto dto, int recordedBy);
    ClinVital? GetVital(long id);
    List<ClinVital> GetPatientVitals(int patientId);

    SchEpisode CreateEpisode(SchEpisode episode);
    SchEpisode? GetEpisode(long id);
    List<SchEpisode> GetPatientEpisodes(int patientId);
    SchEpisode CloseEpisode(long id);

    ClinConsultation CreateConsultation(ClinConsultation consultation);
    ClinConsultation? GetConsultation(long id);
    ClinConsultation? GetActiveConsultationByDoctor(int doctorId);
    List<ClinConsultation> GetPatientConsultations(int patientId);
    ClinConsultation EndConsultation(long id, string clinicalSummary);
    ClinConsultation LinkConsultationToEpisode(long consultationId, long episodeId);
}

public class ClinicalRepository : IClinicalRepository
{
    private readonly HMSContext _context;

    public ClinicalRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }

    public FormTemplateDto CreateFormTemplate(CreateFormTemplateDto dto)
    {
        var template = new ClinFormTemplate
        {
            Title = dto.Title,
            IsActive = dto.IsActive,
            Version = dto.Version
        };

        _context.ClinFormTemplates.Add(template);
        _context.SaveChanges();

        if (dto.Fields != null && dto.Fields.Any())
        {
            var displayOrder = 0;
            foreach (var fieldDto in dto.Fields.OrderBy(f => f.DisplayOrder))
            {
                var formField = new ClinFormField
                {
                    TemplateId = template.Id,
                    FieldId = fieldDto.FieldId,
                    LabelOverride = fieldDto.LabelOverride,
                    DisplayOrder = fieldDto.DisplayOrder,
                    IsRequired = fieldDto.IsRequired,
                    UiControl = fieldDto.UiControl
                };
                _context.ClinFormFields.Add(formField);
                displayOrder++;
            }
            _context.SaveChanges();
        }

        return GetFormTemplate(template.Id)!;
    }

    public FormTemplateDto? GetFormTemplate(int id)
    {
        var template = _context.ClinFormTemplates
            .Include(t => t.ClinFormFields)
                .ThenInclude(f => f.Field)
            .FirstOrDefault(t => t.Id == id);

        if (template == null) return null;

        return new FormTemplateDto
        {
            Id = template.Id,
            Title = template.Title,
            IsActive = template.IsActive ?? true,
            Version = template.Version ?? 1,
            Fields = template.ClinFormFields
                .OrderBy(f => f.DisplayOrder)
                .Select(f => new FormFieldDto
                {
                    Id = f.Id,
                    TemplateId = f.TemplateId ?? 0,
                    FieldId = f.FieldId ?? 0,
                    LabelOverride = f.LabelOverride,
                    DisplayOrder = f.DisplayOrder ?? 0,
                    IsRequired = f.IsRequired ?? false,
                    UiControl = f.UiControl,
                    Field = f.Field != null ? new FieldDefinitionDto
                    {
                        Id = f.Field.Id,
                        FieldCode = f.Field.FieldCode,
                        Name = f.Field.Name,
                        DataType = f.Field.DataType,
                        Unit = f.Field.Unit,
                        ValidationRules = f.Field.ValidationRules,
                        Options = f.Field.Options
                    } : null
                }).ToList()
        };
    }

    public List<FormTemplateDto> GetFormTemplates()
    {
        return _context.ClinFormTemplates
            .Include(t => t.ClinFormFields)
                .ThenInclude(f => f.Field)
            .Select(t => new FormTemplateDto
            {
                Id = t.Id,
                Title = t.Title,
                IsActive = t.IsActive ?? true,
                Version = t.Version ?? 1,
                Fields = t.ClinFormFields
                    .OrderBy(f => f.DisplayOrder)
                    .Select(f => new FormFieldDto
                    {
                        Id = f.Id,
                        TemplateId = f.TemplateId ?? 0,
                        FieldId = f.FieldId ?? 0,
                        LabelOverride = f.LabelOverride,
                        DisplayOrder = f.DisplayOrder ?? 0,
                        IsRequired = f.IsRequired ?? false,
                        UiControl = f.UiControl,
                        Field = f.Field != null ? new FieldDefinitionDto
                        {
                            Id = f.Field.Id,
                            FieldCode = f.Field.FieldCode,
                            Name = f.Field.Name,
                            DataType = f.Field.DataType,
                            Unit = f.Field.Unit,
                            ValidationRules = f.Field.ValidationRules,
                            Options = f.Field.Options
                        } : null
                    }).ToList()
            }).ToList();
    }

    public void DeleteFormTemplate(int id)
    {
        var template = _context.ClinFormTemplates.FirstOrDefault(t => t.Id == id);
        if (template != null)
        {
            _context.ClinFormTemplates.Remove(template);
            _context.SaveChanges();
        }
    }

    public ClinVital CreateVital(VitalsCaptureDto dto, int recordedBy)
    {
        var vital = new ClinVital
        {
            PatientId = dto.PatientId,
            ConsultationId = dto.ConsultationId,
            BpSystolic = dto.BpSystolic,
            BpDiastolic = dto.BpDiastolic,
            HeartRate = dto.HeartRate,
            Temperature = dto.Temperature,
            Spo2 = dto.Spo2,
            RecordedBy = recordedBy,
            RecordedAt = DateTime.UtcNow
        };

        _context.ClinVitals.Add(vital);
        _context.SaveChanges();

        return vital;
    }

    public ClinVital? GetVital(long id)
    {
        return _context.ClinVitals
            .Include(v => v.Patient)
            .Include(v => v.Consultation)
            .Include(v => v.RecordedByNavigation)
            .FirstOrDefault(v => v.Id == id);
    }

    public List<ClinVital> GetPatientVitals(int patientId)
    {
        return _context.ClinVitals
            .Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.RecordedAt)
            .ToList();
    }

    public SchEpisode CreateEpisode(SchEpisode episode)
    {
        _context.SchEpisodes.Add(episode);
        _context.SaveChanges();
        return episode;
    }

    public SchEpisode? GetEpisode(long id)
    {
        return _context.SchEpisodes
            .Include(e => e.Patient)
            .FirstOrDefault(e => e.Id == id);
    }

    public List<SchEpisode> GetPatientEpisodes(int patientId)
    {
        return _context.SchEpisodes
            .Where(e => e.PatientId == patientId)
            .OrderByDescending(e => e.StartDate)
            .ToList();
    }

    public SchEpisode CloseEpisode(long id)
    {
        var episode = _context.SchEpisodes.FirstOrDefault(e => e.Id == id);
        if (episode != null)
        {
            episode.EndDate = DateTime.UtcNow;
            episode.Status = "Closed";
            _context.SaveChanges();
        }
        return episode!;
    }

    public ClinConsultation CreateConsultation(ClinConsultation consultation)
    {
        _context.ClinConsultations.Add(consultation);
        _context.SaveChanges();
        return consultation;
    }

    public ClinConsultation? GetConsultation(long id)
    {
        return _context.ClinConsultations
            .Include(c => c.Patient)
            .Include(c => c.Doctor)
            .Include(c => c.Appointment)
            .Include(c => c.Episode)
            .FirstOrDefault(c => c.Id == id);
    }

    public ClinConsultation? GetActiveConsultationByDoctor(int doctorId)
    {
        return _context.ClinConsultations
            .Include(c => c.Patient)
            .Include(c => c.Appointment)
            .FirstOrDefault(c => c.DoctorId == doctorId && c.StartedAt.HasValue && !c.EndedAt.HasValue);
    }

    public List<ClinConsultation> GetPatientConsultations(int patientId)
    {
        return _context.ClinConsultations
            .Include(c => c.Doctor)
            .Include(c => c.Appointment)
            .Include(c => c.Episode)
            .Where(c => c.PatientId == patientId)
            .OrderByDescending(c => c.StartedAt)
            .ToList();
    }

    public ClinConsultation EndConsultation(long id, string clinicalSummary)
    {
        var consultation = _context.ClinConsultations.FirstOrDefault(c => c.Id == id);
        if (consultation != null)
        {
            consultation.EndedAt = DateTime.UtcNow;
            consultation.ClinicalSummary = clinicalSummary;
            _context.SaveChanges();
        }
        return consultation!;
    }

    public ClinConsultation LinkConsultationToEpisode(long consultationId, long episodeId)
    {
        var consultation = _context.ClinConsultations.FirstOrDefault(c => c.Id == consultationId);
        if (consultation != null)
        {
            consultation.EpisodeId = episodeId;
            _context.SaveChanges();
        }
        return consultation!;
    }
}